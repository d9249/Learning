const form = document.getElementById("form");
const name = document.getElementById("name");
const number = document.getElementById("number");
const address = document.getElementById("location");
const date = document.getElementById("date");
const siteArea = document.getElementById("site-area");
const buildingArea = document.getElementById("building-area");
const yearArea = document.getElementById("year-area");
const maxHeight = document.getElementById("max-height");
const use = document.getElementById("use");
const structure = document.getElementById("structure");
const format = document.getElementById("format");
const facility = document.getElementById("facility");
const floor = document.getElementById("floor");
const grade = document.getElementById("grade");
const result = document.getElementById("result");
const plus = document.getElementById("plus");
const locationMap = document.getElementById("locationMap");
const frontView = document.getElementById("frontView");
const btnSave = document.getElementById("save");

function onChange() {
  if (
    name.value &&
    number.value &&
    address.value &&
    date.value &&
    siteArea.value &&
    buildingArea.value &&
    yearArea.value &&
    maxHeight.value &&
    use.value &&
    structure.value &&
    format.value &&
    facility.value &&
    floor.value &&
    grade.value &&
    result.value &&
    plus.value &&
    locationMap.value.length > 0 &&
    frontView.value.length > 0
  )
    btnSave.disabled = false;
  else {
    btnSave.disabled = true;
  }
}
