const input = document.querySelector(".crack-input");
const preview = document.querySelector(".preview");
const blankImage = document.querySelector(".blank-img");

input.addEventListener("change", updateImageDisplay);

function validFileType(file) {
  return fileTypes.includes(file.type);
}

function updateImageDisplay() {
  while (preview.firstChild) {
    preview.removeChild(preview.firstChild);
  }

  const curFiles = input.files;
  if (curFiles.length === 0) {
    const para = document.createElement("p");
    para.textContent = "선택한 파일을 업로드 할 수 없습니다.";
    preview.appendChild(para);
  } else {
    const list = document.createElement("ol");
    preview.appendChild(list);

    for (const file of curFiles) {
      const listItem = document.createElement("li");
      const para = document.createElement("p");

      if (validFileType(file)) {
        const image = document.createElement("img");
        image.src = URL.createObjectURL(file);
        listItem.appendChild(image);
        blankImage.style.display = "none";
      } else {
        para.textContent = `File name ${file.name} : 파일의 형식이 옳지 않습니다.`;
        console.log(file.name);
        listItem.appendChild(para);
      }

      list.appendChild(listItem);
    }
  }
}
