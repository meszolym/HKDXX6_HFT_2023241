let precincts = [];
let officers = [];
let cases = [];
let precinctCaseStats = [];

let precinctIdForUpdate = -1;
let officerIdForUpdate = -1;
let caseIdForUpdate = -1;

let connection;
setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:33410/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on('CaseCreated', (user, message) => { getCaseData(); })
    connection.on('CaseUpdated', (user, message) => { getCaseData() });
    connection.on('CaseDeleted', (user, message) => { getCaseData() });
    connection.on('PrecinctCreated', (user, message) => { getPrecinctData(); });
    connection.on('PrecinctUpdated', (user, message) => { getPrecinctData(); });
    connection.on('PrecinctDeleted', (user, message) => { getPrecinctData(); });
    connection.on('OfficerCreated', (user, message) => { getOfficerData(); });
    connection.on('OfficerUpdated', (user, message) => { getOfficerData(); });
    connection.on('OfficerDeleted', (user, message) => { getOfficerData(); });

    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
    } catch (error) {
        console.log(error);
        setTimeout(start, 5000);
    }
}

class Case {
    id;
    name;
    description;
    openedAt;
    closedAt;
    isClosed;
    openTimeSpan;
    officerOnCaseID;
    officerOnCase;

}

class Officer {
    badgeNo;
    firstName;
    lastName;
    rank;
    directCO_badgeNo;
    directCO;
    precinctID;
    precinct;
    hireDate;
}

class Precinct {
    id;
    address;
}

class TimeSpan {
    ticks;
    days;
    hours;
    milliseconds;
    minutes;
    seconds;
    totalDays;
    totalHours;
    totalMilliseconds;
    totalMinutes;
    totalSeconds;
}

getPrecinctData();
getOfficerData();
getCaseData();

function showMain() {
    document.getElementById('welcomeDiv').style.display = 'block';
    document.getElementById('precinctDiv').style.display = 'none';
    document.getElementById('officerDiv').style.display = 'none';
    document.getElementById('caseDiv').style.display = 'none';
    document.getElementById('statisticsDiv').style.display = 'none';
}

// #region Precinct
function showPrecincts() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctDiv').style.display = 'block';
    resetPrecinctMenu();
    document.getElementById('officerDiv').style.display = 'none';
    document.getElementById('caseDiv').style.display = 'none';
    document.getElementById('statisticsDiv').style.display = 'none';

    getPrecinctData();
}

async function getPrecinctData() {
    await fetch('http://localhost:33410/precinct')
        .then(x => x.json())
        .then(y => {
            precincts = y
            displayPrecinct();
        });
}

function displayPrecinct() {
    document.getElementById('precinctsTableBody').innerHTML = '';

    precincts.forEach(p => {
        document.getElementById('precinctsTableBody').innerHTML +=
            `<tr>
                <td>${p.id}</td>
                <td>${p.address}</td>
                <td><button class="pure-button" type="button" onclick=showEditPrecinct('${p.id}')>Edit</button><button class="pure-button" type="button" onclick=removePrecinct('${p.id}')>Delete</button></td>
            </tr>`
    });

}

function removePrecinct(id) {
    fetch('http://localhost:33410/precinct/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: null
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
        else {
            getPrecinctData();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

}

function showAddPrecinct() {
    document.getElementById('precinctContentDiv').style.display = 'none';
    document.getElementById('precinctAddDiv').style.display = 'inline-grid';
}

function addPrecinct() {

    let idInput = document.getElementById('addPrecinctID').value;
    let addressInput = document.getElementById('addPrecinctAddress').value;

    fetch('http://localhost:33410/precinct', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { id: idInput, address: addressInput }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    resetPrecinctMenu();
    getPrecinctData();
}

function showEditPrecinct(id) {
    document.getElementById('precinctContentDiv').style.display = 'none';
    document.getElementById('precinctEditDiv').style.display = 'inline-grid';

    let p = precincts.find(p => p.id == id);
    precinctIdForUpdate = id;

    document.getElementById('editPrecinctID').value = id;
    document.getElementById('editPrecinctAddress').value = p.address;
}

function editPrecinct() {
    
    let addressInput = document.getElementById('editPrecinctAddress').value;

    fetch('http://localhost:33410/precinct', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { id: precinctIdForUpdate, address: addressInput }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    precinctIdForUpdate = -1;
    resetPrecinctMenu();
    getPrecinctData();
}

function resetPrecinctMenu() {
    document.getElementById('addPrecinctID').value = '';
    document.getElementById('addPrecinctAddress').value = '';

    document.getElementById('editPrecinctID').value = '';
    document.getElementById('editPrecinctAddress').value = '';
    precinctIdForUpdate = -1;

    document.getElementById('precinctAddDiv').style.display = 'none';
    document.getElementById('precinctEditDiv').style.display = 'none';
    document.getElementById('precinctContentDiv').style.display = 'block';
}

// #endregion

// #region Officer
function showOfficers() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctDiv').style.display = 'none';
    document.getElementById('officerDiv').style.display = 'block';
    resetOfficerMenu();
    document.getElementById('caseDiv').style.display = 'none';
    document.getElementById('statisticsDiv').style.display = 'none';
    getOfficerData();
}

async function getOfficerData() {
    await fetch('http://localhost:33410/officer')
        .then(x => x.json())
        .then(y => {
            officers = y
            displayOfficer();
        });
}

function displayOfficer() {
    document.getElementById('officersTableBody').innerHTML = '';

    officers.forEach(o => {
        let elementRow = `<tr>
                <td>${o.badgeNo}</td>
                <td>${Ranks[o.rank]}</td>
                <td>${o.firstName}</td>
                <td>${o.lastName}</td>
                <td>${o.precinctID}</td>`;



        if (o.directCO != null) {
            elementRow += `<td>${Ranks[o.directCO.rank]} ${o.directCO.firstName} ${o.directCO.lastName} (${o.directCO.badgeNo})</td>`;
        }
        else {
            elementRow += `<td></td>`;
        }
        let date = new Date(o.hireDate);
        elementRow += `<td>${date.toDateString()}.</td>
                <td><button class="pure-button" type="button" onclick=showEditOfficer('${o.badgeNo}')>Edit</button><button class="pure-button" type="button" onclick=removeOfficer('${o.badgeNo}')>Delete</button></td>
                </tr>`;

        document.getElementById('officersTableBody').innerHTML += elementRow;
    });
}

const Ranks = {
    1: 'Recruit',
    2: 'Patrol officer',
    3: 'Detective',
    4: 'Sergeant',
    5: 'Lieutenant',
    6: 'Captain'
}

function removeOfficer(id) {
    fetch('http://localhost:33410/officer/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: null
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
        else {
            getOfficerData();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

}

function showAddOfficer() {
    document.getElementById('officerContentDiv').style.display = 'none';
    document.getElementById('officerAddDiv').style.display = 'inline-grid';

    document.getElementById('addPrecinctSelector').innerHTML = '';
    precincts.forEach(p => {
        document.getElementById('addPrecinctSelector').innerHTML +=
            `<option value='${p.id}'>${p.id} (${p.address})</option>`;
    });

    selectedPrecinctID = document.getElementById('addPrecinctSelector').value;

    document.getElementById('addCoSelector').innerHTML = "<option value=null selected='selected'></option>";
    officers.filter(o => o.precinctID == selectedPrecinctID).forEach(o => {
        document.getElementById('addCoSelector').innerHTML +=
            `<option value='${o.badgeNo}'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
    })

    let ranksList = Object.keys(Ranks);

    if (officers.filter(o => o.precinctID == selectedPrecinctID).some(o => o.rank == 6)) {
        ranksList = ranksList.filter(r => r != 6)
    }

    document.getElementById('addRankSelector').innerHTML = '';
    ranksList.forEach(r => {
        document.getElementById('addRankSelector').innerHTML +=
            `<option value='${r}'>${Ranks[r]}</option>`;
    });

    var now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    document.getElementById('addOfficerHireDate').value = now.toISOString().slice(0, 16).split('T')[0];
}

function addPrecinctSelectionChanged() {
    precinctID = document.getElementById('addPrecinctSelector').value;

    document.getElementById('addCoSelector').innerHTML = "<option value=null selected='selected'></option>";
    officers.filter(o => o.precinctID == precinctID).forEach(o => {

        document.getElementById('addCoSelector').innerHTML +=
            `<option value='${o.badgeNo}'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;

    });

    let ranksList = Object.keys(Ranks);

    if (officers.filter(o => o.precinctID == precinctID).some(o => o.rank == 6)) {
        ranksList = ranksList.filter(r => r != 6)
    }

    document.getElementById('addRankSelector').innerHTML = '';
    ranksList.forEach(r => {
        document.getElementById('addRankSelector').innerHTML +=
            `<option value='${r}'>${Ranks[r]}</option>`;
    });

}
function addOfficer() {

    inputFirstName = document.getElementById('addOfficerFirstName').value;
    inputLastName = document.getElementById('addOfficerLastName').value;
    inputHireDate = document.getElementById('addOfficerHireDate').value;
    inputPrecinctID = document.getElementById('addPrecinctSelector').value;
    inputCoID = document.getElementById('addCoSelector').value;
    inputRankID = document.getElementById('addRankSelector').value;

    fetch('http://localhost:33410/officer', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { firstName: inputFirstName, lastName: inputLastName, hireDate: inputHireDate, precinctID: inputPrecinctID, directCO_badgeNo: inputCoID, rank: inputRankID }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    resetOfficerMenu();
    getOfficerData();


}

function showEditOfficer(id) {
    document.getElementById('officerContentDiv').style.display = 'none';
    document.getElementById('officerEditDiv').style.display = 'inline-grid';

    officer = officers.find(t => t.badgeNo == id);
    officerIdForUpdate = id;

    document.getElementById('editOfficerFirstName').value = officer.firstName;
    document.getElementById('editOfficerLastName').value = officer.lastName;
    document.getElementById('editOfficerHireDate').value = officer.hireDate.split('T')[0];

    document.getElementById('editPrecinctSelector').innerHTML = '';
    precincts.forEach(p => {
        if (p.id == officer.precinctID) {
            document.getElementById('editPrecinctSelector').innerHTML +=
                `<option value='${p.id}' selected='selected'>${p.id} (${p.address})</option>`;
        }
        else {
            document.getElementById('editPrecinctSelector').innerHTML +=
                `<option value='${p.id}'>${p.id} (${p.address})</option>`;
        }
    });

    document.getElementById('editCoSelector').innerHTML = '<option value=null></option>';
    officers.filter(o => o.precinctID == officer.precinctID && o.badgeNo != officerIdForUpdate).forEach(o => {
        if (officer.directCO != null && o.badgeNo == officer.directCO.badgeNo) {
            document.getElementById('editCoSelector').innerHTML +=
                `<option value='${o.badgeNo}' selected='selected'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
        }
        else {
            document.getElementById('editCoSelector').innerHTML +=
                `<option value='${o.badgeNo}'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
        }
    });

    let ranksList = null;
    ranksList = Object.keys(Ranks);

    if (officers.filter(o => o.rank == 6 && o.precinctID == officer.precinctID).some(o=> o.badgeNo != officerIdForUpdate)) {
        ranksList = ranksList.filter(r => r != 6)
    }

    document.getElementById('editRankSelector').innerHTML = '';
    ranksList.forEach(r => {
        if (officer.rank == r) {
            document.getElementById('editRankSelector').innerHTML +=
                `<option value='${r}' selected='selected'>${Ranks[r]}</option>`;
        }
        else {
            document.getElementById('editRankSelector').innerHTML +=
                `<option value='${r}'>${Ranks[r]}</option>`;
        }
        
    });

}

function editPrecinctSelectionChanged() {
    officer = officers.find(t => t.badgeNo == officerIdForUpdate);

    precinctID = document.getElementById('editPrecinctSelector').value;

    document.getElementById('editCoSelector').innerHTML = '<option value=null></option>';
    officers.filter(o => o.precinctID == precinctID && o.badgeNo != officerIdForUpdate).forEach(o => {

        if (officer.directCO != null && o.badgeNo == officer.directCO.badgeNo) {
            document.getElementById('editCoSelector').innerHTML +=
                `<option value='${o.badgeNo}' selected='selected'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
        }
        else {
            document.getElementById('editCoSelector').innerHTML +=
                `<option value='${o.badgeNo}'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
        }
        
    });

    let ranksList = null;
    ranksList = Object.keys(Ranks);

    if (officers.filter(o => o.rank == 6 && o.precinctID == precinctID).some(o => o.badgeNo != officerIdForUpdate)) {
        ranksList = ranksList.filter(r => r != 6)
    }
    
    document.getElementById('editRankSelector').innerHTML = '';
    ranksList.forEach(r => {
        if (officer.rank == r) {
            document.getElementById('editRankSelector').innerHTML +=
                `<option value='${r}' selected='selected'>${Ranks[r]}</option>`;
        }
        else {
            document.getElementById('editRankSelector').innerHTML +=
                `<option value='${r}'>${Ranks[r]}</option>`;
        }
    });
}

function editOfficer() {

    inputFirstName = document.getElementById('editOfficerFirstName').value;
    inputLastName = document.getElementById('editOfficerLastName').value;
    inputHireDate = document.getElementById('editOfficerHireDate').value;
    inputPrecinctID = document.getElementById('editPrecinctSelector').value;
    inputCoID = document.getElementById('editCoSelector').value;
    inputRankID = document.getElementById('editRankSelector').value;

    fetch('http://localhost:33410/officer', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { badgeNo: officerIdForUpdate, firstName: inputFirstName, lastName: inputLastName, hireDate: inputHireDate, precinctID: inputPrecinctID, directCO_badgeNo: inputCoID, rank: inputRankID }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    officerIdForUpdate = -1;
    resetOfficerMenu();
    getOfficerData();

}

function resetOfficerMenu() {

    document.getElementById('addOfficerFirstName').value = '';
    document.getElementById('addOfficerLastName').value = '';
    document.getElementById('addOfficerHireDate').value = '';
    document.getElementById('addPrecinctSelector').value = '';
    document.getElementById('addCoSelector').value = '';
    document.getElementById('addRankSelector').value = '';

    document.getElementById('editOfficerFirstName').value = '';
    document.getElementById('editOfficerLastName').value = '';
    document.getElementById('editOfficerHireDate').value = '';
    document.getElementById('editPrecinctSelector').value = '';
    document.getElementById('editCoSelector').value = '';
    document.getElementById('editRankSelector').value = '';
    officerIdForUpdate = -1;

    document.getElementById('officerAddDiv').style.display = 'none';
    document.getElementById('officerEditDiv').style.display = 'none';
    document.getElementById('officerContentDiv').style.display = 'block';
}

// #endregion

// #region Case
function showCases() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctDiv').style.display = 'none';
    document.getElementById('officerDiv').style.display = 'none';
    document.getElementById('caseDiv').style.display = 'block';
    document.getElementById('statisticsDiv').style.display = 'none';
    resetCaseMenu();
    getCaseData();
}

async function getCaseData() {
    await fetch('http://localhost:33410/case')
        .then(x => x.json())
        .then(y => {
            cases = y
            displayCase();
        });
}

function displayCase() {
    document.getElementById('casesTableBody').innerHTML = '';

    cases.forEach(c => {
        let elementRow = `<tr>
            <td>${c.name}</td>`;

        if (c.officerOnCase != null) {
            elementRow += `<td>${c.officerOnCase.precinctID}</td>
                <td>${Ranks[c.officerOnCase.rank]} ${c.officerOnCase.firstName} ${c.officerOnCase.lastName} (${c.officerOnCase.badgeNo})</td>`;
            
        } else {
            elementRow += `<td></td><td></td>`;
        }
        let openedAt = new Date(c.openedAt);
        elementRow += `<td>${openedAt.toDateString()}</td>
            <td>${c.isClosed}</td>`;

        if (c.isClosed) {
            let closedAt = new Date(c.closedAt);
            elementRow += `
            <td>${closedAt.toDateString()}</td>
            <td>${timeSpanString(c.openTimeSpan)}</td>
            `;
        } else {
            elementRow += `<td></td><td></td>`
        }
            

        elementRow += `<td>`;

        if (c.officerOnCaseID == null) {
            elementRow += `<button class="pure-button" type="button" onclick=showAutoAssignCase('${c.id}')>AutoAssign</button>`
        } else {
            elementRow += `<button disabled class=pure-button type="button">AutoAssign</button>`
        }

        if (c.isClosed) {
            elementRow += `<button class="pure-button" type="button" onclick=openCase('${c.id}')>Open</button><button class="pure-button" type="button" disabled>Edit</button>`
        } else {
            elementRow += `<button class="pure-button" type="button" disabled>Open</button><button class="pure-button" type="button" onclick=showEditCase('${c.id}')>Edit</button>`
        }

        elementRow += `<button class="pure-button" type="button" onclick=showCaseDescription('${c.id}')>Desc.</button><button class="pure-button" type="button" onclick=removeCase('${c.id}')>Delete</button></td>
        </tr>`

        elementRow += `<tr id='desc_${c.id}' style="display:none; border-top: 0px"><td colspan=8><table class="descTable"><thead><th>Description</th></thead><tbody><td>${c.description}</td></tbody></table></td></tr>`

        document.getElementById('casesTableBody').innerHTML += elementRow;
    })
}

function timeSpanString(timespan) {
    if (timespan.includes('.')) {
        days = timespan.split('.')[0];
        hhmmss = timespan.split('.')[1];

        return `${days} days, ${hhmmss}`;
    }
    return timespan;
}

function removeCase(id) {
    fetch('http://localhost:33410/case/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: null
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
        else {
            getCaseData();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });
}

function showAddCase() {
    document.getElementById('caseAddDiv').style.display = 'inline-grid';
    document.getElementById('caseEditDiv').style.display = 'none';
    document.getElementById('caseAutoAssignDiv').style.display = 'none';
    document.getElementById('caseContentDiv').style.display = 'none';

    document.getElementById('addCaseOfficerSelect').innerHTML = '';
    document.getElementById('addCaseOfficerSelect').innerHTML += '<option value=null></option>';

    officers.forEach(o => {
        document.getElementById('addCaseOfficerSelect').innerHTML +=
            `<option value=${o.badgeNo}>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
    });

    var now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    document.getElementById('addCaseOpenedAt').value = now.toISOString().slice(0, 16);
}

function addCase() {

    let inputName = document.getElementById('addCaseName').value;
    let inputOfficerId = document.getElementById('addCaseOfficerSelect').value;
    let inputOpenedAt = document.getElementById('addCaseOpenedAt').value;
    let inputClosedAt = document.getElementById('addCaseClosedAt').value;
    let inputDescription = document.getElementById('addCaseDescription').value;

    if (inputClosedAt == '') {
        inputClosedAt = null;
    }

    if (inputOfficerId == 'null' ) {
        inputOfficerId = null;
    }

    fetch('http://localhost:33410/case', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { name: inputName, description: inputDescription, openedAt: inputOpenedAt, closedAt: inputClosedAt, officerOnCaseID: inputOfficerId }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    resetCaseMenu();
    getCaseData();
}

function showEditCase(id) {
    document.getElementById('caseAddDiv').style.display = 'none';
    document.getElementById('caseEditDiv').style.display = 'inline-grid';
    document.getElementById('caseAutoAssignDiv').style.display = 'none';
    document.getElementById('caseContentDiv').style.display = 'none';


    let updateCase = cases.find(c => c.id == id);
    caseIdForUpdate = id;

    document.getElementById('editCaseName').value = updateCase.name;

    document.getElementById('editCaseOpenedAt').value = updateCase.openedAt;


    document.getElementById('editCaseOfficerSelect').innerHTML = '';
    document.getElementById('editCaseOfficerSelect').innerHTML += '<option value=null></option>';

    officers.forEach(o => {
        if (updateCase.officerOnCaseID == o.badgeNo) {
            document.getElementById('editCaseOfficerSelect').innerHTML +=
                `<option value=${o.badgeNo} selected='selected'>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
        }
        else {
            document.getElementById('editCaseOfficerSelect').innerHTML +=
                `<option value=${o.badgeNo}>${Ranks[o.rank]} ${o.firstName} ${o.lastName} (${o.badgeNo})</option>`;
        }
        
    });

    document.getElementById('editCaseClosedAt').value = updateCase.closedAt;

    document.getElementById('editCaseDescription').value = updateCase.description;

}

function editCase() {
    let inputName = document.getElementById('editCaseName').value;
    let inputOfficerId = document.getElementById('editCaseOfficerSelect').value;
    let inputOpenedAt = document.getElementById('editCaseOpenedAt').value;
    let inputClosedAt = document.getElementById('editCaseClosedAt').value;
    let inputDescription = document.getElementById('editCaseDescription').value;

    if (inputClosedAt == '') {
        inputClosedAt = null;
    }

    if (inputOfficerId == 'null') {
        inputOfficerId = null;
    }

    fetch('http://localhost:33410/case', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { id: caseIdForUpdate, name: inputName, description: inputDescription, openedAt: inputOpenedAt, closedAt: inputClosedAt, officerOnCaseID: inputOfficerId }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    caseIdForUpdate = -1;
    resetCaseMenu();
    getCaseData();

}

function openCase(id) {

    let openedCase = cases.find(c => c.id == id);

    fetch('http://localhost:33410/case', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { id: id, name: openedCase.name, description: openedCase.description, openedAt: openedCase.openedAt, closedAt: null, officerOnCaseID: openedCase.officerOnCaseID }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    getCaseData();
}

function showAutoAssignCase(id) {
    let caseToAssign = cases.find(c => c.id == id);
    caseIdForUpdate = id;

    document.getElementById('caseAddDiv').style.display = 'none';
    document.getElementById('caseEditDiv').style.display = 'none';
    document.getElementById('caseAutoAssignDiv').style.display = 'inline-grid';
    document.getElementById('caseContentDiv').style.display = 'none';

    document.getElementById('autoAssignCasePrecinctSelect').innerHTML = '';

    document.getElementById('autoAssignCaseName').value = caseToAssign.name;
    precincts.forEach(p => {
        document.getElementById('autoAssignCasePrecinctSelect').innerHTML += `<option value='${p.id}'>${p.id} (${p.address})</option>`;
    })
}

function autoAssignCase() {

    let inputPrecinctID = document.getElementById('autoAssignCasePrecinctSelect').value;

    console.log(JSON.stringify(
        { caseID: caseIdForUpdate, precinctID: inputPrecinctID }
    ));

    fetch('http://localhost:33410/case/autoassign', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(
            { caseID: caseIdForUpdate, precinctID: inputPrecinctID }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
    }).then(data => {
        if (data != undefined) {
            console.log(data);
            if (data.msg != undefined) {
                throw new Error(data.msg);
            }
            if (data.status != undefined && data.status != 200) {
                throw new Error(data.title)
            }
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

    caseIdForUpdate = -1;
    resetCaseMenu();
    getCaseData();
}

function showCaseDescription(id) {
    cases.filter(c => c.id != id).forEach(c => {
        document.getElementById(`desc_${c.id}`).style.display = 'none';
    })

    if (document.getElementById(`desc_${id}`).style.display == 'none') {
        document.getElementById(`desc_${id}`).style.display = 'revert';
    }
    else {
        document.getElementById(`desc_${id}`).style.display = 'none';
    }

    
}

function resetCaseMenu() {

    document.getElementById('addCaseName').value = '';
    document.getElementById('addCaseOfficerSelect').value = '';
    document.getElementById('addCaseOpenedAt').value = '';
    document.getElementById('addCaseClosedAt').value = '';
    document.getElementById('addCaseDescription').value = '';

    document.getElementById('editCaseName').value = '';
    document.getElementById('editCaseOfficerSelect').value = '';
    document.getElementById('editCaseOpenedAt').value = '';
    document.getElementById('editCaseClosedAt').value = '';
    document.getElementById('editCaseDescription').value = '';
    
    document.getElementById('autoAssignCaseName').value = '';
    document.getElementById('autoAssignCasePrecinctSelect').value = '';

    caseIdForUpdate = -1;

    document.getElementById('caseAddDiv').style.display = 'none';
    document.getElementById('caseEditDiv').style.display = 'none';
    document.getElementById('caseAutoAssignDiv').style.display = 'none';
    document.getElementById('caseContentDiv').style.display = 'block';
}

// #endregion

// #region stats

async function getPrecinctCaseStats() {
    await fetch('http://localhost:33410/Statistics/casesPerPrecinctStatistics')
        .then(x => x.json())
        .then(y => {
            precinctCaseStats = y
            drawCharts();
        });
}

function showStatistics() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctDiv').style.display = 'none';
    document.getElementById('officerDiv').style.display = 'none';
    document.getElementById('caseDiv').style.display = 'none';
    document.getElementById('statisticsDiv').style.display = 'block';

    getPrecinctCaseStats();
}

let openCasesPerPrecinctChart;
let closedCasesPerPrecinctChart;

function drawCharts() {

    //precincts
    let xValues = [];

    //number
    let yValuesClosed = [];
    let yValuesOpen = [];

    precinctCaseStats.forEach(s => {
        xValues.push(`#${s.precinct.id}`);
        yValuesClosed.push(s.closedCases);
        yValuesOpen.push(s.openCases);
    });

    if (openCasesPerPrecinctChart != undefined) {
        openCasesPerPrecinctChart.destroy();
    }
    if (closedCasesPerPrecinctChart != undefined) {
        closedCasesPerPrecinctChart.destroy();
    }
    

    let barColors = "lightblue"

    openCasesPerPrecinctChart = new Chart("OpenCasesPerPrecinct", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValuesOpen
            }]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: "Open cases per precinct"
            },
            scales: {
                yAxes: [{
                    ticks: {beginAtZero: true}
                }]
            }
        }
    });

    closedCasesPerPrecinctChart = new Chart("ClosedCasesPerPrecinct", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValuesClosed
            }]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: "Closed cases per precinct"
            },
            scales: {
                yAxes: [{
                    ticks: { beginAtZero: true }
                }]
            }
        }
    });
}

//#endregion