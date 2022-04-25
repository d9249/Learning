function fileCheck() {
  const file = document.getElementById("formFileLg").value;
  const button = document.getElementById("button");
  if (!file) {
    button.disabled = true;
  } else {
    button.disabled = false;
  }
}
