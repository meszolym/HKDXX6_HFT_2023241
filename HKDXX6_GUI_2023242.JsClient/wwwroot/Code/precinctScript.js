let precincts = [];

getdata();

async function getdata() {
    await fetch('http://localhost:33410/precinct')
        .then(x => x.json())
        .then(y => {
            precincts = y
            display();
        });
}
function display() {
    document.getElementById('tableBody').innerHTML = '';

    precincts.forEach(p => {
        document.getElementById('tableBody').innerHTML +=
            `<tr>
                <td>${p.id}</td>
                <td>${p.address}</td>
                <td>
                    <button class="pure-button" type="button" onclick=remove('${p.id}')>Delete</button>
                    <button class="pure-button" type="button" onclick=edit('${p}')>Edit</button>
                </td>
            </tr>`
    });

}

function add() {
    let idInput = document.getElementById('idInput').value;
    let addressInput = document.getElementById('addressInput').value;

    fetch('http://localhost:33410/precinct', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            {
                id: idInput,
                address: addressInput
            }
        )
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
        else {
            getdata();
            display();
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

    document.getElementById('idInput').value = "";
    document.getElementById('addressInput').value = "";

}
function remove(id) {
    fetch('http://localhost:33410/precinct/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    }).then(response => {
        if (!response.ok) {
            return response.json();
        }
        else {
            getdata();
            display();
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