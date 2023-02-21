const {app, BrowserWindow} = require('electron')
const path = require('path')
const { ipcMain } = require('electron')
var edge = require("electron-edge-js");

let mainWindow;
const createWindow = () => {
    mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: {
          preload: path.join(__dirname, 'preload.js')
        }
    })
  mainWindow.loadFile('index.html')
}

app.whenReady().then(() => {
  createWindow()

  app.on('activate', function () {
    if (BrowserWindow.getAllWindows().length === 0) createWindow()
  })
})

app.on('window-all-closed', function () {
  if (process.platform !== 'darwin') app.quit()
})

var getData = edge.func(require('path').join(__dirname, 'registry-writer.cs'));

ipcMain.handle('custom-endpoint', async (event, data) => {
    getData(data, function (error, result) {
        if (error) throw error;
        mainWindow.webContents.send('custom-endpoint', result);
    });
})