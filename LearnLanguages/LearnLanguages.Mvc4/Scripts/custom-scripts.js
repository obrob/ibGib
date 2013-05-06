
$(document).ready(function () {
  AttachAccordion();
});

//Attaches accordion to all classes marked accordion
function AttachAccordion() {
  $("#accordion").accordion({ collapsible: true, heightStyle: "content" });
}

