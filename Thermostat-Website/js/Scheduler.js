var table = document.getElementById("scheduleTable");;
var Olddata = [];

async function updateTable() {
    fetch('http://192.168.1.1:5000/Schedule/All', {
            headers: {
                'Access-Control-Allow-Origin': '*'
            }
        })
        .then(response => response.json())
        .then(data => {
            if (Olddata.length != data.length) {
                table = document.getElementById("scheduleTable");
                table.innerHTML = "";
                data.forEach(item => {
                    let row = table.insertRow();
                    let date = row.insertCell(0);
                    date.innerHTML = item.hour + ":" + item.minute;
                    let name = row.insertCell(1);
                    name.innerHTML = item.temperature + "°C";
                    let del = row.insertCell(2);
                    del.innerHTML = ' <button style="color:black; padding: 0" class="bi bi-trash"> </button>'
                    del.onclick = function(e) {
                        e.stopPropagation();
                        deleteItem(item.guid);
                    }
                })
                let row = table.insertRow();
                let date = row.insertCell(0);
                date.innerHTML = '<input id="scheduleTime" type="time" required>';
                let temp = row.insertCell(1);
                temp.innerHTML = '<input id="scheduleTemp" type="number" min="15" max="25" required>';
                let apply = row.insertCell(2);
                apply.innerHTML = '<button style="color:black; padding: 0" class="bi bi-check" onclick="addItem(event)"> </button>';

                Olddata = data;
            }
        });

}

async function deleteItem(guid) {
    const options = {
        method: 'DELETE',
        headers: {

            'Content-Type': 'application/json'
        }
    }
    return await fetch('http://192.168.1.1:5000/Schedule/Delete?guid=' + guid, options).then(response => {
        updateTable();
        return response
    })
}

async function addItem(e) {
    e.stopPropagation();
    let data = {
        hour: parseInt(document.getElementById("scheduleTime").value.split(":")[0]),
        minute: parseInt(document.getElementById("scheduleTime").value.split(":")[1]),
        temperature: parseInt(document.getElementById("scheduleTemp").value),
    }
    return await fetch('http://192.168.1.1:5000/Schedule/New', {
        method: 'POST',
        mode: 'cors',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data),
    }).then(response => {
        response.text().then(function(text) {});
    })
}

setInterval(updateTable, 1000);
updateTable();