
function showMain() {
    document.getElementById('welcomeDiv').style.display = 'block';
    document.getElementById('precinctsDiv').style.display = 'none';
    document.getElementById('officersDiv').style.display = 'none';
    document.getElementById('casesDiv').style.display = 'none';
}

function showPrecincts() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctsDiv').style.display = 'block';
    document.getElementById('officersDiv').style.display = 'none';
    document.getElementById('casesDiv').style.display = 'none';
}

function showOfficers() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctsDiv').style.display = 'none';
    document.getElementById('officersDiv').style.display = 'block';
    document.getElementById('casesDiv').style.display = 'none';
}

function showCases() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctsDiv').style.display = 'none';
    document.getElementById('officersDiv').style.display = 'none';
    document.getElementById('casesDiv').style.display = 'block';
}