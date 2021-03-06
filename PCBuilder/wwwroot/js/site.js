// Chat Bot

$(document).ready(function () {
    $('.chat_icon').click(function () {
        $('.chat_box').toggleClass('active');
    });

    $('.conv-form-wrapper').convform({ selectInputStyle: 'disable' })
});



// Adding aditional specifications

function add(tpe) {
    var specificationlabel = document.createElement("label");
    var specificationinput = document.createElement("input");

    specificationlabel.id = "inputSpecifications";
    specificationlabel.textContent = "Name"
    specificationlabel.name = "Model.Specifications[i].Title"

    specificationinput.className = "form-control";
    specificationinput.type = "text";
    specificationinput.placeholder = "Specification..";
    specificationinput.name = "Model.Specifications[i].Title";
    /*--------------------------------------------------------------*/

    var descriptionlabel = document.createElement("label");
    var descriptioninput = document.createElement("input");

    descriptionlabel.id = "inputSpecifications";
    descriptionlabel.textContent = "Description";
    descriptionlabel.name = "Model.Specifications[i].Description"

    descriptioninput.className = "form-control";
    descriptioninput.type = "text";
    descriptioninput.placeholder = "Description..";
    descriptioninput.name = "Model.Specifications[i].Description";
    /*--------------------------------------------------------------*/

    var foo = document.getElementById("specifications");
    foo.appendChild(specificationlabel);
    foo.appendChild(specificationinput);

    foo.appendChild(descriptionlabel);
    foo.appendChild(descriptioninput);
}
document.getElementById("create-specification").onclick = function () {
    add();
};
