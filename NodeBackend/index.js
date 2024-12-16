const cors = require("cors");
const express = require("express");
const mongoose = require("mongoose");
const multer = require("multer");

const AdminModel = require("./models/Admin");
const FriendsModel = require("./models/Friends");
const LobbyModel = require("./models/Lobby");
const KillDeathModel = require("./models/killdeath");
const MessagesModel = require("./models/Messages");
const ToxicsModel = require("./models/Toxic");
const TransactionsModel = require("./models/Transactions");
const UserModel = require("./models/Users");

const app = express();
app.use(cors());
app.use(express.json());
app.use(express.static("public"));

const storage = multer.diskStorage({
    destination: function (req, file, cb) {
        return cb(null, "public/Images");
    },
    filename: function (req, file, cb) {
        return cb(null, `${Date.now()}_${file.originalname}`);
    },
});

const upload = multer({ storage });

mongoose.connect("mongodb://127.0.0.1:27017/FURY");
const bodyParser = require("body-parser");

app.use(bodyParser.urlencoded({ extended: true }));

// Admin Login
app.post("/api/Admin", (req, res) => {
    // console.log(req.body);
    AdminModel.findOne(req.body)
        .then((users) => {
            if (users != null) {
                // console.log(users);
                res.json(users);
            } else {
                res.json("Invalid username or password");
            }
        })
        .catch((err) => res.json(err));
});

// Edit Admin Email
app.patch("/api/EditAdmin/:username", (req, res) => {
    getUsername = req.params.username;
    _email = req.body._email;
    AdminModel.updateOne({ username: getUsername }, { email: _email })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Edit Admin Password
app.patch("/api/EditAdminPassword/:username", (req, res) => {
    getUsername = req.params.username;
    _oldPassword = req.body._oldPassword;
    _newPassword = req.body._newPassword;
    _confirmPassword = req.body._confirmPassword;
    if (_newPassword === _confirmPassword) {
        AdminModel.findOne({ username: getUsername })
            .then(
                AdminModel.updateOne(
                    { password: _oldPassword },
                    { password: _newPassword }
                )
                    .then((users) => res.json(users))
                    .catch((err) => res.json(err))
            )
            .catch((err) => res.json(err));
    } else {
        res.json("Password not match");
    }
});

// Admin Dashboard
app.get("/api/Dashboard/:username", (req, res) => {
    const _username = req.params.username;
    AdminModel.findOne({ username: _username })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// User count Dashboard
app.get("/api/UserCount", (req, res) => {
    // Ensure model is instantiated
    UserModel.find()
        .count()
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Coins count Dashboard
app.get("/api/TotalCoins", (req, res) => {
    // Ensure model is instantiated
    TransactionsModel.aggregate([
        {
            $group: { _id: null, total_coins: { $sum: "$fury_coins" } },
        },
    ])
        .then((coins) => res.json(coins))
        .catch((err) => res.json(err));
});

// Transaction count Dashboard
app.get("/api/TransactionCount", (req, res) => {
    // Ensure model is instantiated
    TransactionsModel.find()
        .count()
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Total Users Details
app.get("/api/Users", (req, res) => {
    UserModel.find(req.body)
        .then((users) => {
            res.json(users);
        })
        .catch((err) => res.json(err));
});

//Account Deactivate
app.post("/api/btn-Deactivate", (req, res) => {
    var _username = req.body.friendUsername;
    // console.log(_username);
    UserModel.updateOne({ username: _username }, { account_status: 0 })
        .then((users) => {
            res.json(users);
        })
        .catch((err) => res.json(err));
});

//Account Activate
app.post("/api/btn-Activate", (req, res) => {
    var _username = req.body.friendUsername;
    UserModel.updateOne({ username: _username }, { account_status: 1 })
        .then((users) => {
            res.json(users);
        })
        .catch((err) => res.json(err));
});

// Transactions Lists
app.get("/api/Transactions", (req, res) => {
    TransactionsModel.find(req.body)
        .then((transactions) => {
            res.json(transactions);
        })
        .catch((err) => res.json(err));
});

// Transaction Cancel
app.post("/api/btn-Tcancel", (req, res) => {
    var _username = req.body._username;
    var _transaction_code = req.body._transaction_code;
    var _fury_coins = req.body._fury_coins;
    var _date = req.body._date;
    var _time = req.body._time;
    // console.log(_username);
    TransactionsModel.updateOne(
        {
            username: _username,
            transaction_code: _transaction_code,
            fury_coins: _fury_coins,
            date: _date,
            time: _time,
        },
        { account_status: 0 }
    )
        .then((users) => {
            res.json(users);
        })
        .catch((err) => res.json(err));
});

// Transaction Activate
app.post("/api/btn-Tactivate", (req, res) => {
    var _username = req.body._username;
    var _transaction_code = req.body._transaction_code;
    var _fury_coins = req.body._fury_coins;
    var _date = req.body._date;
    var _time = req.body._time;
    const _activate_by = req.body.sessUsername;
    console.log("[#]");
    console.log(_activate_by);
    // var id = req.body._id;
    console.log(_username + _transaction_code + _fury_coins + _date + _time);
    TransactionsModel.updateOne(
        {
            // _id: id,
            $and: [
                { username: _username },
                { transaction_code: _transaction_code },
                { fury_coins: _fury_coins },
                { date: _date },
                { time: _time },
            ],
        },
        { activate: 1, activate_by: _activate_by }
    )
        .then((users) => {
            res.json(_activate_by);
        })
        .catch((err) => res.json(err));
});

// Friend Lists
// app.post("/api/FriendLists/", (req, res) => {
//     const _username = req.body.getUsername;
//     var friends_username = [];
//     FriendsModel.find({
//         username2: _username,
//         approve1: 1,
//         approve2: 1,
//     })
//         .then((friends) => {
//             const usernames1 = friends.map((friend) => friend.username1); // Extract username1
//             // console.log(usernames1); // Array of usernames1 from each friend
//             // res.json(usernames1);
//             friends_username = friends_username.concat(usernames1);

//             FriendsModel.find({
//                 username1: _username,
//                 approve1: 1,
//                 approve2: 1,
//             })
//                 .then((friends) => {
//                     const usernames2 = friends.map(
//                         (friend) => friend.username2
//                     ); // Extract username1
//                     // console.log(usernames2); // Array of usernames1 from each friend
//                     // res.json(usernames2);
//                     friends_username = friends_username.concat(usernames2);
//                     // console.log(friends_username);
//                     // res.json(friends_username);

//                     UserModel.find({ username: friends_username })
//                         .then((users) => {
//                             // console.log(users);
//                             res.json(users);
//                         })
//                         .catch((err) => res.json(err));
//                 })
//                 .catch((err) => res.json(err));
//         })
//         .catch((err) => res.json(err));

//     // console.log(friends_username);
// });

// Users
// Register
app.post("/api/Register", (req, res) => {
    UserModel.create(req.body)
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// apply profile
app.patch("/api/UploadProfile/:username", upload.single("file"), (req, res) => {
    const _username = req.params.username;
    console.log("[*] " + _username);
    // console.log(req.body);
    console.log(req.file.filename);
    UserModel.updateOne({ username: _username }, { profile: req.file.filename })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Login
app.post("/api/Login", (req, res) => {
    var _username = req.body.username;
    var _password = req.body.password;
    UserModel.findOne({ username: _username, password: _password })
        .then((users) => {
            if (users != null) {
                res.json(users);
            } else {
                res.json("Invalid username or password");
            }
        })
        .catch((err) => res.json(err));
});

// Unity Login
app.post("/UnityLogin", (req, res) => {
    const _username = req.body.unityUsername;
    const _password = req.body.unityPassword;
    // var _password = "";
    console.log("[*] " + _username + " " + _password);
    UserModel.findOne({ username: _username, password: _password })
        .then((users) => {
            if (users != null) {
                // console.log(req.body);
                UserModel.updateOne(
                    { username: _username },
                    { $inc: { login_counter: 1 } }
                )
                    .then((users1) => {
                        UserModel.findOne({
                            username: _username,
                            password: _password,
                        })
                            .then((users) => {
                                if (users != null) {
                                    res.json(users);
                                } else {
                                    res.json("Invalid username or password");
                                }
                            })
                            .catch((err) => res.json(err));
                    })
                    .catch((err) => res.json(err));
                // res.json(users);
            } else {
                res.json("Invalid username or password");
            }
        })
        .catch((err) => res.json(err));
});

// Edit Users
app.patch("/api/EditUser/:username", (req, res) => {
    getUsername = req.params.username;
    _email = req.body._email;
    _phone = req.body._phone;
    _dob = req.body._dob;
    UserModel.updateOne(
        { username: getUsername },
        { email: _email, phone: _phone, dob: _dob }
    )
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Edit Password
app.patch("/api/EditPassword/:username", (req, res) => {
    getUsername = req.params.username;
    _oldPassword = req.body._oldPassword;
    _newPassword = req.body._newPassword;
    _confirmPassword = req.body._confirmPassword;
    if (_newPassword === _confirmPassword) {
        UserModel.findOne({ username: getUsername })
            .then(
                UserModel.updateOne(
                    { password: _oldPassword },
                    { password: _newPassword }
                )
                    .then((users) => res.json(users))
                    .catch((err) => res.json(err))
            )
            .catch((err) => res.json(err));
    } else {
        res.json("Password not match");
    }
});

//User Hero Stats for Home
app.get("/api/Home/:username", (req, res) => {
    // console.log(req.params.username);
    const _username = req.params.username;
    UserModel.findOne({ username: _username })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Friends Profile
app.get("/api/Profile/:username", (req, res) => {
    const _username = req.params.username;
    UserModel.findOne({ username: _username })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Friend Lists
app.post("/api/FriendLists/", (req, res) => {
    const _username = req.body.getUsername;
    var friends_username = [];
    FriendsModel.find({
        username2: _username,
        approve1: 1,
        approve2: 1,
    })
        .then((friends) => {
            const usernames1 = friends.map((friend) => friend.username1); // Extract username1
            // console.log(usernames1); // Array of usernames1 from each friend
            // res.json(usernames1);
            friends_username = friends_username.concat(usernames1);

            FriendsModel.find({
                username1: _username,
                approve1: 1,
                approve2: 1,
            })
                .then((friends) => {
                    const usernames2 = friends.map(
                        (friend) => friend.username2
                    ); // Extract username1
                    // console.log(usernames2); // Array of usernames1 from each friend
                    // res.json(usernames2);
                    friends_username = friends_username.concat(usernames2);
                    // console.log(friends_username);
                    // res.json(friends_username);

                    UserModel.find({ username: friends_username })
                        .then((users) => {
                            // console.log(users);
                            res.json(users);
                        })
                        .catch((err) => res.json(err));
                })
                .catch((err) => res.json(err));
        })
        .catch((err) => res.json(err));

    // console.log(friends_username);
});

// Friend Requests
app.post("/api/FriendRequests/", (req, res) => {
    const _username = req.body.getUsername;
    var friends_username = [];

    FriendsModel.find({
        username2: _username,
        approve1: 1,
        approve2: 0,
    })
        .then((friends) => {
            const usernames1 = friends.map((friend) => friend.username1); // Extract username1
            // console.log(usernames2); // Array of usernames1 from each friend
            // res.json(usernames2);
            friends_username = friends_username.concat(usernames1);
            // console.log(friends_username);
            // res.json(friends_username);

            UserModel.find({ username: friends_username })
                .then((users) => {
                    // console.log(users);
                    res.json(users);
                })
                .catch((err) => res.json(err));
        })
        .catch((err) => res.json(err));

    // console.log(friends_username);
});

// Sent Requests
app.post("/api/SentRequests/", (req, res) => {
    const _username = req.body.getUsername;
    var friends_username = [];

    FriendsModel.find({
        username1: _username,
        approve1: 1,
        approve2: 0,
    })
        .then((friends) => {
            const usernames2 = friends.map((friend) => friend.username2); // Extract username1
            // console.log(usernames2); // Array of usernames1 from each friend
            // res.json(usernames2);
            friends_username = friends_username.concat(usernames2);
            // console.log(friends_username);
            // res.json(friends_username);

            UserModel.find({ username: friends_username })
                .then((users) => {
                    // console.log(users);
                    res.json(users);
                })
                .catch((err) => res.json(err));
        })
        .catch((err) => res.json(err));

    // console.log(friends_username);
});

// Add Friends
app.post("/api/AddFriends/", (req, res) => {
    const _username = req.body.getUsername;
    var friends_username = [];
    FriendsModel.find({
        username2: _username,
    })
        .then((friends) => {
            const usernames1 = friends.map((friend) => friend.username1); // Extract username1
            // console.log(usernames1); // Array of usernames1 from each friend
            // res.json(usernames1);
            friends_username = friends_username.concat(usernames1);

            FriendsModel.find({
                username1: _username,
            })
                .then((friends) => {
                    const usernames2 = friends.map(
                        (friend) => friend.username2
                    ); // Extract username1
                    // console.log(usernames2); // Array of usernames1 from each friend
                    // res.json(usernames2);
                    friends_username = friends_username.concat(usernames2);
                    friends_username = friends_username.concat(_username);
                    // console.log(friends_username);
                    // res.json(friends_username);
                    // Get all users (excluding friends_username)
                    UserModel.find({ username: { $nin: friends_username } }) // $ne: not equal to
                        .then((users) => {
                            res.json(users);
                        })
                        .catch((err) => res.json(err));
                })
                .catch((err) => res.json(err));
        })
        .catch((err) => res.json(err));
});

// btn-AddFriends
app.post("/api/btn-AddFriends", (req, res) => {
    FriendsModel.create(req.body)
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// btn-AcceptRequests
app.post("/api/btn-AcceptRequests/", (req, res) => {
    const sessUsername = req.body.getUsername;
    const friendUsername = req.body.friendUsername;
    const _approve2 = req.body.approve2;
    FriendsModel.updateMany(
        {
            username1: friendUsername,
            username2: sessUsername,
        },
        { approve2: _approve2 }
    )
        .then((users) => {
            // console.log(sessUsername);
            // console.log(friendUsername);
            res.json(users);
        })
        .catch((err) => res.json(err));
});

// btn-CancelRequests
// btn-Unfriend Requests
app.post("/api/btn-UnfriendRequests/", (req, res) => {
    const sessUsername = req.body.getUsername;
    const friendUsername = req.body.friendUsername;

    // console.log(sessUsername);
    // console.log(friendUsername);
    var output;

    FriendsModel.deleteMany({
        username1: sessUsername,
        username2: friendUsername,
    })
        .then((users) => {
            // console.log(sessUsername);
            // console.log(friendUsername);
            output += users;
        })
        .catch((err) => res.json(err));
    FriendsModel.deleteMany({
        username2: sessUsername,
        username1: friendUsername,
    })
        .then((users) => {
            // console.log(sessUsername);
            // console.log(friendUsername);
            output += users;
            res.json(output);
        })
        .catch((err) => res.json(err));
});

// sent Message
app.post("/api/SentMessage/", (req, res) => {
    MessagesModel.create(req.body)
        .then((messageText) => {
            res.json(messageText);
        })
        .catch((err) => res.json(err));
});

// sent Message
app.post("/api/GetMessage/", (req, res) => {
    var getUsername = req.body.getUsername;
    var friendUsername = req.body.friendUsername;
    MessagesModel.find({
        $or: [
            { sender: getUsername, receiver: friendUsername },
            { sender: friendUsername, receiver: getUsername },
        ],
    })
        .then((message) => {
            res.json(message);
        })
        .catch((err) => res.json(err));
});

// Toxic
app.get("/api/Toxics/", (req, res) => {
    ToxicsModel.find()
        .then((toxics) => {
            const toxics_text = toxics.map((toxic) => toxic.toxics_text);
            // const censoredMessage = censorMessage(
            //     message.body.message_text,
            //     toxics_message.body.toxics_text
            // );
            res.json(toxics_text);
        })
        .catch((err) => res.json(err));
});

// app.post("/api/Friends/", (req, res) => {
//     const _username = req.body.getUsername;
//     UserModel.find({ username: { $ne: _username } })
//         .then((users) => {
//             // res.json(users);
//             FriendsModel.find({
//                 $or: [
//                     {
//                         username1: _username,
//                         approve1: 1,
//                         approve2: 1,
//                     },
//                     {
//                         username2: _username,
//                         approve1: 1,
//                         approve2: 1,
//                     },
//                 ],
//             }).then((e) => {
//                 console.log(e);
//                 res.json(users);
//             });
//             //     .catch((err) => res.json(err));
//         })
//         .catch((err) => res.json(err));
// });

app.get("/", (req, res) => {
    UserModel.find({})
        .then((users) => res.json(users))
        .catch((err) => res.log(err));
});

app.post("/createUser", (req, res) => {
    UserModel.create(req.body)
        .then((users) => {
            res.json(users);
            console.log(users);
        })
        .catch((err) => res.json(err));
});

app.get("/getUser/:id", (req, res) => {
    const id = req.params.id;
    UserModel.findById({ _id: id })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

app.put("/updateUser/:id", (req, res) => {
    const id = req.params.id;
    UserModel.findByIdAndUpdate(
        { _id: id },
        { name: req.body.name, email: req.body.email, age: req.body.age }
    )
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

app.delete("/deleteUser/:id", (req, res) => {
    const id = req.params.id;
    UserModel.findByIdAndDelete({ _id: id })
        .then((users) => res.json(users))
        .catch((err) => res.json(err));
});

// Unity Kill and Death
app.post("/UnityKillDeath", (req, res) => {
    const killer = req.body.killer;
    const victim = req.body.victim;
    KillDeathModel.create(req.body)
        .then((users) => {
            res.json(users);
            console.log(users);
        })
        .catch((err) => res.json(err));
});

// Unity create Lobby
app.post("/UnityCreateLobby", (req, res) => {
    const room_id = req.body.room_id;
    const datetime = req.body.datetime;
    const game_mode = req.body.game_mode;
    const game_map = req.body.game_map;
    const host_username = req.body.host_username;
    LobbyModel.create(req.body)
        .then((respond) => {
            res.json(respond);
            console.log(respond);
        })
        .catch((err) => res.json(err));
});

// Unity create Lobby
app.post("/UnityJoinLobby", (req, res) => {
    const room_id = req.body.room_id;
    const player_user_id = req.body.player_user_id;
    const player_username = req.body.player_username;
    // console.log(room_id);
    // console.log(player_user_id);
    // console.log(player_username);
    LobbyModel.updateMany(
        { room_id: room_id },
        {
            $push: {
                player: { user_id: player_user_id, username: player_username },
            },
        }
    )
        .then((respond) => {
            var match_count = respond.matchedCount;
            console.log("[*] " + match_count);
            LobbyModel.findOne({ room_id: room_id })
                .then((response1) => {
                    res.json(match_count + " " + response1.host_username);
                    console.log(match_count + " " + response1.host_username);
                })
                .catch((err) => res.json(err));
        })
        .catch((err) => res.json(err));
});

// Hide canvas and Start Game after Clicking join button
app.get("/getLastRoomId/:lobby_id", (req, res) => {
    lobby_id = req.params.lobby_id;
    console.log(lobby_id);
    // Retrieve the most recent document by sorting in descending order based on `createdAt`
    LobbyModel.findOne({})
        .sort({ datetime: -1 }) // Sort by createdAt in descending order
        .then((lastLobby) => {
            if (lastLobby.room_id == lobby_id) {
                res.json(lastLobby.room_id);
            } else {
                res.json("Incorrect ID");
            }
        })
        .catch((err) => res.status(500).json({ error: err.message }));
});

// Stats
app.get("/api/totalGamePlayed/:playerUsername", (req, res) => {
    var playerUsernameCount = 0;
    // Ensure model is instantiated
    LobbyModel.find()
        .then((data) => {
            // console.log(data);
            data.forEach((item) => {
                // Iterate through the 'player' array in each object
                item.player.forEach((player) => {
                    if (player.username === req.params.playerUsername) {
                        playerUsernameCount++;
                        // console.log("Username found:", player.username);
                        // Access other properties of the player object
                        // console.log("User ID:", player.user_id);
                        // ...
                    }
                });
            });
            res.json(playerUsernameCount);
        })
        .catch((err) => res.json(err));
});

app.get("/api/totalLobbyCreated/:username", (req, res) => {
    // Ensure model is instantiated
    LobbyModel.find({ host_username: req.params.username })
        .count()
        .then((data) => res.json(data))
        .catch((err) => res.json(err));
});

app.get("/api/totalGameTable/:playerUsername", (req, res) => {
    var concatResultJson = [];
    // Ensure model is instantiated
    LobbyModel.find()
        .then((data) => {
            // console.log(data);
            data.forEach((item) => {
                // Iterate through the 'player' array in each object
                item.player.forEach((player) => {
                    if (player.username === req.params.playerUsername) {
                        // console.log(data);
                        concatResultJson = concatResultJson.concat(item);
                        // console.log("Username found:", player.username);
                        // Access other properties of the player object
                        // console.log("User ID:", player.user_id);
                        // ...
                    }
                });
            });
            res.json(concatResultJson);
            console.log(concatResultJson);
        })
        .catch((err) => res.json(err));
});

app.listen(3001, () => {
    console.log("Server is running");
});
