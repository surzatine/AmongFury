const mongoose = require("mongoose");

// Register
const UserSchema = new mongoose.Schema({
    profile: String,
    username: String,
    email: String,
    password: String,
    phone: Number,
    dob: String,
    ip: String,
    win: Number,
    lose: Number,
    kill: Number,
    death: Number,
    create_date: String,
    create_time: String,
    account_status: Number,
    online: Number,
    transaction: String,
    coins: Number,
    public_key: String,
    private_key: String,
    login_counter: Number,
});

const UserModel = mongoose.model("users", UserSchema);
module.exports = UserModel;
