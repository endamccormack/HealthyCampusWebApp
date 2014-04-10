var ingredientsRow;
var tagTextBox;
var tagSelectList;
var tagTextCounter = 1;
var tagSelectCounter = 1;
var ingredientsCounter = 1;

function addTagDropDownList() {
    var newDd = tagSelectList.clone();
    newDd.attr("name", newDd.attr("name") + tagSelectCounter);
    newDd.attr("id", newDd.attr("id") + tagSelectCounter);
    $("#addCurrentTabContainer").append(newDd);
    tagSelectCounter++;
}

function addTagTextboxForNewTag() {
    var newTb = tagTextBox.clone();
    newTb.attr("name", newTb.attr("name") + tagTextCounter);
    newTb.attr("id", newTb.attr("id") + tagTextCounter);
    $("#addNewTagContainer").append(newTb);
    tagTextCounter++
}

function addIngredientsRow() {
    var newRow = ingredientsRow.clone();
    for (var i = 0; i < 3; i++) {
        $(newRow.children().children()[i]).attr("name", $(newRow.children().children()[i]).attr("name") + ingredientsCounter);
        $(newRow.children().children()[i]).attr("id", $(newRow.children().children()[i]).attr("id") + ingredientsCounter);
    }
    ingredientsCounter++;
    $('#ingredientsTable').append(newRow);
}

function showMeasurements() {
    $('#measurementsTable').slideToggle();
}

$(document).ready(function () {
    $('#measurementsTable').toggle();
    tagTextBox = $('#tag').clone();
    tagSelectList = $('#Tags').clone();
    ingredientsRow = $($('#ingredientsTable tr')[1]).clone();
});
