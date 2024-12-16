const express = require("express");

const { connectToDb, getDb } = require("./db");

const app = express();

app.use(express.json());

let db;

connectToDb((err) => {
    if (!err) {
        app.listen(3001, () => {
            console.log("Running in 3001");
        });

        db = getDb();
    }
});

// Get students data of 10
app.get("/api/students", (req, res) => {
    const page = req.query.p || 0;
    const studentPerPage = 10;

    let students = [];
    db.collection("mock_data")
        .find()
        .sort({ id: 1 })
        .skip(page * studentPerPage)
        .limit(studentPerPage)
        .forEach((student) => students.push(student))
        .then(() => {
            res.status(200).json(students);
        })
        .catch(() => {
            res.status(500).json({
                msg: " Internal Server Error / Connection to db failed",
            });
        });
});

// Get students from id
app.get("/api/students/:id", (req, res) => {
    const studentID = parseInt(req.params.id);
    if (!isNaN(studentID)) {
        db.collection("mock_data")
            .findOne({ id: studentID })
            .then((student) => {
                if (student) {
                    res.status(200).json(student);
                } else {
                    res.status(400).json({ msg: "Student not found" });
                }
            })
            .catch(() => {
                res.status(500).json({ msg: "Error getting student info" });
            });
    } else {
        res.status(400).json({ Error: "  Bad Request / ID must be int" });
    }
});

// Insert data in database
app.post("/api/students", (req, res) => {
    const student = req.body;
    db.collection("mock_data")
        .insertOne(student)
        .then((result) => {
            res.status(201).json({ result });
        })
        .catch(() => {
            res.status(500).json({ msg: "Error creating Students" });
        });
});

// update
app.patch("/api/students/:id", (req, res) => {
    let updates = req.body;
    const studentID = parseInt(req.params.id);
    if (!isNaN(studentID)) {
        db.collection("mock_data")
            .updateOne({ id: studentID }, { $set: updates })
            .then((result) => {
                res.status(200).json({ result });
            })
            .catch(() => {
                res.status(500).json({ msg: "Error Update Studetnt" });
            });
    } else {
        res.status(400).json({ Error: "Student Id must be number" });
    }
});

// Delete
app.delete("/api/students/:id", (req, res) => {
    const studentID = parseInt(req.params.id);
    if (!isNaN(studentID)) {
        db.collection("mock_data")
            .deleteOne({ id: studentID })
            .then((result) => {
                res.status(200).json({ result });
            })
            .catch(() => {
                res.status(500).json({ msg: "Error Delete Student" });
            });
    } else {
        res.status(400).json({ Error: "Student ID must be Int" });
    }
});
