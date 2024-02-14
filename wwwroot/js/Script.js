function initializeTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl, {
            trigger: 'hover'
        });
    });
}


function initializeScrollSpy() {
    var dataSpyEl = document.querySelector('[data-bs-spy="scroll"]');
    if (dataSpyEl) {
        new bootstrap.ScrollSpy(dataSpyEl, {
            target: '#navbar'
        });
    }
}

function openCv(url) {
    window.open(url, '_blank');
}

function scrollToSection(sectionId) {
    var element = document.getElementById(sectionId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
    }
}

function showAlert(message) {
    window.alert(message);
}