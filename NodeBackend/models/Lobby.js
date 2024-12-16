const mongoose = require("mongoose");

// Register
const LobbySchema = new mongoose.Schema({
    room_id: String,
    datetime: String,
    game_mode: String,
    game_map: String,
    host_username: String,
    player: [
        {
            user_id: String,
            username: String,
        },
    ],
});

const LobbyModel = mongoose.model("lobbys", LobbySchema);
module.exports = LobbyModel;
