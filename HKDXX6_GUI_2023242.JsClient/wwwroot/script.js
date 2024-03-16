let precincts = [];
let officers = [];
let cases = [];

function showMain() {
    document.getElementById('welcomeDiv').style.display = 'block';
    document.getElementById('precinctsDiv').style.display = 'none';
    document.getElementById('officersDiv').style.display = 'none';
    document.getElementById('casesDiv').style.display = 'none';
}

// #region Precinct
function showPrecincts() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctsDiv').style.display = 'block';
    document.getElementById('officersDiv').style.display = 'none';
    document.getElementById('casesDiv').style.display = 'none';

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
                <td>
                    <button class="pure-button" type="button" onclick=removePrecinct('${p.id}')>Delete</button>
                    <button class="pure-button" type="button" onclick=editPrecinct('${p}')>Edit</button>
                </td>
            </tr>`
    });

}

function removePrecinct(id) {
    fetch('http://localhost:33410/precinct/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
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
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

}

// #endregion

// #region Officer
function showOfficers() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctsDiv').style.display = 'none';
    document.getElementById('officersDiv').style.display = 'block';
    document.getElementById('casesDiv').style.display = 'none';
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
                <td>
                    <button class="pure-button" type="button" onclick=removeOfficer('${o.badgeNo}')>Delete</button>
                    <button class="pure-button" type="button" onclick=editOfficer('${o}') > Edit</button >
                </td></tr>`;

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
        headers: { 'Content-Type': 'application/json', },
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
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

}

// #endregion

// #region Case
function showCases() {
    document.getElementById('welcomeDiv').style.display = 'none';
    document.getElementById('precinctsDiv').style.display = 'none';
    document.getElementById('officersDiv').style.display = 'none';
    document.getElementById('casesDiv').style.display = 'block';
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
            

        elementRow += `<td>
                    <button class="pure-button" type="button" onclick=removeCase('${c.id}')>Delete</button>
                    <button class="pure-button" type="button" onclick=editCase('${c}') > Edit</button >
                </td></tr>`

        document.getElementById('casesTableBody').innerHTML += elementRow;
    })
}

function timeSpanString(timespan) {
    days = timespan.split('.')[0];
    hhmmss = timespan.split('.')[1];

    return `${days} days, ${hhmmss}`;
}

function removeCase(id) {
    fetch('http://localhost:33410/case/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
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
        }
    }).catch(error => {
        console.error(error);
        alert(error.message);
    });

}

// #endregion