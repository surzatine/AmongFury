const mongoose = require("mongoose");

// Register
const KillDeathSchema = new mongoose.Schema({
    killer: String,
    victim: String,
    room_id: String,
    date: Date,
    time: String,
});

const KillDeathModel = mongoose.model("killdeaths", KillDeathSchema);
module.exports = KillDeathModel;
