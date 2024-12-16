const mongoose = require("mongoose");
// Messages
const MessagesSchema = new mongoose.Schema({
    sender: String,
    receiver: String,
    message_text: String,
    date: String,
    time: String,
    message_counter: Number,
    sender_refresh: Number,
    receiver_refresh: Number,
});

const MessagesModel = mongoose.model("messages", MessagesSchema);
module.exports = MessagesModel;
