// Function to change language
function changeLanxD(language) {
    console.log("Language selected:", language); // Check if this logs when the dropdown changes
    localStorage.setItem('lang', language);
    console.log("Updated localStorage lang:", localStorage.getItem('lang')); // Should print the updated language
    loadLanguage(language);
}

// Function to load the selected language JSON file
function loadLanguage(language) {
    console.log("Loading language:", language); // Debugging line

    fetch(`/translations/${language}.json`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to load language file');
            }
            return response.json();
        })
        .then(data => {
            applyTranslations(data);
        })
        .catch(error => {
            console.error('Error loading language file:', error);
        });
}

// Function to apply translations to the page
function applyTranslations(translations) {
    console.log("Applying translations:", translations); // Debugging line

    Object.keys(translations).forEach(key => {
        const element = document.getElementById(key);
        if (element) {
            element.innerText = translations[key];
        }
    });
}

// Ensure language is set when the page is fully loaded
window.addEventListener("load", () => {
    const currentLanguage = localStorage.getItem('lang') || 'en'; // Default to English
    console.log("Using language:", currentLanguage); // Check the loaded language
    loadLanguage(currentLanguage);


});

document.getElementById("language-select").addEventListener("change", function() {
    const language = this.value;
    console.log("Language selected via event listener:", language); // Debugging line
    localStorage.setItem('lang', language);
    loadLanguage(language);
});

