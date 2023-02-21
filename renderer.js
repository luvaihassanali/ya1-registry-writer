const button = document.getElementById('jbtn');
const input = document.getElementById('jdata');

$(document).ready(function () {
    console.log("ready!")
})

window.api.handle('custom-endpoint', (event, d) => function (event, d) {
    input.value = d;
}, event)

button.addEventListener('click', submitButton);

function submitButton() {
    var tmp = input.value;
    input.value = "";
    window.api.send('custom-endpoint', tmp);
}