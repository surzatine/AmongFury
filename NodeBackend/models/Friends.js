const mongoose = require("mongoose");
// Friends
const FriendsSchema = new mongoose.Schema({
    username1: String,
    username2: String,
    approve1: Number,
    approve2: Number,
});

const FriendsModel = mongoose.model("friends", FriendsSchema);
module.exports = FriendsModel;
