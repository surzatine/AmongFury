const express = require("express");
const multer = require("multer");
const cors = require("cors");
const mongoose = require("mongoose");

const app = express();
app.use(cors());
app.use(express.json());
app.use(express.static("public"));

const storage = multer.diskStorage({
    destination: function (req, file, cb) {
        return cb(null, "Images");
    },
    filename: function (req, file, cb) {
        return cb(null, `${Date.now()}_${file.originalname}`);
    },
});

const upload = multer({ storage });

mongoose.connect("mongodb://127.0.0.1:27017/FURY");

app.get("/", (req, res) => {
    res.json("lol");
});

app.post("/api/Register", (req, res) => {
    console.log(req.body);
    res.json("lol");
});

app.post("/api/File/", upload.single("file"), (req, res) => {
    console.log("[*]");
    console.log(req.body);
    console.log(req.file.filename);
    res.json("upload");
});
// apply profile
app.patch("/api/UploadProfile/:username", upload.single("file"), (req, res) => {
    const _username = req.params.username;
    console.log("[*] " + _username);
    console.log(req.body);
    console.log(req.file.filename);
    res.json("upload");
});
app.listen(3001);
