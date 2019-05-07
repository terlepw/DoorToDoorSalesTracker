const profileShowMessage = "Show Update Profile Form";
const profileHideMessage = "Hide Update Profile Form";

const profileFormToggleElement = document.getElementById("show-hide-profile-form");
const profileFormElement = document.getElementById("update-profile-form");
profileFormElement.hidden = !holdProfileForm;

const passwordShowMessage = "Show Change Password Form";
const passwordHideMessage = "Hide Change Password Form";

const passwordFormToggleElement = document.getElementById("show-hide-password-form");
const passwordFormElement = document.getElementById("reset-password-form");
passwordFormElement.hidden = !holdPasswordForm;

function toggleForm(event, showMessage, hideMessage, form, toggle) {
    event.stopPropagation();

    form.toggleAttribute("hidden");

    if (form.hidden) {
        toggle.innerText = showMessage;
    }
    else {
        toggle.innerText = hideMessage;
    }
}

profileFormToggleElement.addEventListener('click', (event) => {
    toggleForm(event, profileShowMessage, profileHideMessage, profileFormElement, profileFormToggleElement);
});

passwordFormToggleElement.addEventListener('click', (event) => {
    toggleForm(event, passwordShowMessage, passwordHideMessage, passwordFormElement, passwordFormToggleElement);
});