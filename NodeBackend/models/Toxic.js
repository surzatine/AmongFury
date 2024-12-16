const mongoose = require("mongoose");
// Toxics
const ToxicsSchema = new mongoose.Schema({
    toxics_text: String,
});

const ToxicsModel = mongoose.model("toxics", ToxicsSchema);
module.exports = ToxicsModel;
