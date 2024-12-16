const mongoose = require("mongoose");
// Transactions
const TransactionsSchema = new mongoose.Schema({
    username: String,
    transaction_code: String,
    fury_coins: Number,
    date: String,
    time: String,
    activate: Number,
    activate_by: String,
});

const TransactionsModel = mongoose.model("Transactions", TransactionsSchema);
module.exports = TransactionsModel;
