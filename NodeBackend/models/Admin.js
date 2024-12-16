const mongoose = require("mongoose");

const AdminSchema = new mongoose.Schema({
    username: String,
    email: String,
    password: String,
});

const AdminModel = mongoose.model("admin_users", AdminSchema);
module.exports = AdminModel;
