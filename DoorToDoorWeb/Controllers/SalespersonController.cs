using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorToDoorLibrary.DAL;
using DoorToDoorLibrary.DatabaseObjects;
using DoorToDoorWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoorToDoorWeb.Controllers
{
    public class SalespersonController : AuthController
    {
        public SalespersonController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {

        }

        private SalespersonDashboardViewModel CreateSalespersonDashboardViewModel()
        {
            SalespersonDashboardViewModel dashboardViewModel = new SalespersonDashboardViewModel();
            dashboardViewModel.BestCustomersByCount = _db.GetSalesmanDashboardCount(CurrentUser.Id);
            dashboardViewModel.BestCustomersByRevenue = _db.GetSalesmanDashboardRevenue(CurrentUser.Id);
            dashboardViewModel.MySales = _db.GetSalesmanDashboardSales(CurrentUser.Id);
            dashboardViewModel.MyRevenue = _db.GetSalesmanDashboardTotalRevenue(CurrentUser.Id);

            return dashboardViewModel;
        }

        private SalespersonHousesListViewModel CreateSalespersonHousesListViewModel()
        {
            SalespersonHousesListViewModel houseListModel = new SalespersonHousesListViewModel();
            houseListModel.Houses = _db.GetAassignedHouses(CurrentUser.Id);

            return houseListModel;
        }

        private HouseDetailsViewModel CreateHouseDetailsViewModel(int houseID)
        {
            HouseDetailsViewModel model = new HouseDetailsViewModel();

            model.House = _db.GetHouse(houseID);
            model.Notes = _db.GetHouseNotes(houseID);
            model.AddNote = new AddHouseNoteViewModel();
            model.StatusOptions = _db.GetHouseStatusOptions(model.House.StatusID);
            model.Residents = _db.GetHouseResidents(houseID);
            model.AddResident = new AddResidentViewModel();

            return model;
        }

        private TransactionsViewModel CreateTransactionsViewModel()
        {
            TransactionsViewModel model = new TransactionsViewModel();

            model.Transactions = _db.GetTransactions(CurrentUser.Id);
            model.AddTransaction = new AddTransactionViewModel();
            model.HouseList = _db.GetSalesTransactionHouseOptions(CurrentUser.Id);
            model.ProductList = _db.GetMyProductOptions(CurrentUser.Id);

            return model;
        }

        [HttpGet]
        public IActionResult Home()
        {
            ActionResult result = GetAuthenticatedView("Home", CreateSalespersonDashboardViewModel());

            if (Role.IsSalesperson)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult Houses()
        {
            ActionResult result = GetAuthenticatedView("Houses", CreateSalespersonHousesListViewModel());

            if (Role.IsSalesperson)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult HouseDetails(int houseID)
        {
            HouseDetailsViewModel model = CreateHouseDetailsViewModel(houseID);

            ActionResult result = GetAuthenticatedView("HouseDetails", model);
            
            if (!Role.IsSalesperson)
            {
                result = RedirectToAction("Login", "Home");
            }
            else if (model.House.AssignedSalespersonID != CurrentUser.Id)
            {
                ModelState.AddModelError("not-your-house", "You do not have permission to see this house");
                result = View("Houses", CreateSalespersonHousesListViewModel());
            }

            return result;
        }

        [HttpPost]
        public ActionResult AddHouseNote(HouseDetailsViewModel model)
        {
            ActionResult result = View("HouseDetails", CreateHouseDetailsViewModel(model.AddNote.HouseID));
            TempData["holdNoteForm"] = true;

            if (Role.IsSalesperson)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        NoteItem newNote = new NoteItem()
                        {
                            HouseID = model.AddNote.HouseID,
                            UserID = CurrentUser.Id,
                            Note = model.AddNote.Note,
                            SubmittedDate = DateTime.Now
                        };

                        _db.AddHouseNote(newNote);

                        TempData["holdNoteForm"] = false;

                        result = RedirectToAction("HouseDetails", new { houseID = model.AddNote.HouseID });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid-note", ex.Message);
                }
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }

            return result;
        }

        [HttpPost]
        public ActionResult AddHouseResident(HouseDetailsViewModel model)
        {
            ActionResult result = View("HouseDetails", CreateHouseDetailsViewModel(model.AddResident.HouseID));
            TempData["holdResidentForm"] = true;

            if (Role.IsSalesperson)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        ResidentItem newResident = new ResidentItem()
                        {
                            HouseID = model.AddResident.HouseID,
                            FirstName = model.AddResident.FirstName,
                            LastName = model.AddResident.LastName
                        };

                        _db.AddHouseResident(newResident);

                        TempData["holdResidentForm"] = false;

                        result = RedirectToAction("HouseDetails", new { houseID = model.AddResident.HouseID });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid-resident", ex.Message);
                }
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }

            return result;
        }

        [HttpPost]
        public ActionResult SetHouseStatus([FromBody]SetHouseStatusViewModel model)
        {
            bool statusUpdateSuccess = false;
            ActionResult result = null;
            if (Role.IsSalesperson)
            {
                try
                {
                    statusUpdateSuccess = _db.SetHouseStatus(model.HouseID, model.StatusID, CurrentUser.Id);

                    result = GetAuthenticatedJson(Json(new HouseStatusUpdateJsonResponseModel(statusUpdateSuccess)), true);
                }
                catch (Exception ex)
                {
                    result = GetAuthenticatedJson(Json(new HouseStatusUpdateJsonResponseModel(ex.Message)), true);
                }
            }
            else
            {
                result = GetAuthenticatedJson(null, false);
            }

            return result;
        }

        [HttpGet]
        public ActionResult Transactions()
        {
            ActionResult result = GetAuthenticatedView("Transactions", CreateTransactionsViewModel());

            if (Role.IsSalesperson)
            {
                return result;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddTransaction(TransactionsViewModel model)
        {
            ActionResult result = View("Transactions", CreateTransactionsViewModel());
            TempData["holdForm"] = true;

            if (Role.IsSalesperson)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        SalesTransactionItem newTransaction = new SalesTransactionItem()
                        {
                            Date = DateTime.Now,
                            Amount = model.AddTransaction.Amount,
                            SalesmanID = CurrentUser.Id,
                            HouseID = model.AddTransaction.HouseID,
                            ProductID = model.AddTransaction.ProductID

                        };

                        _db.AddTransaction(newTransaction);

                        TempData["holdForm"] = false;

                        result = RedirectToAction("Transactions");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("invalid", ex.Message);
                }
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }

            return result;
        }
    }
}