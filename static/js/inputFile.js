const fileTypes = [
  "image/apng",
  "image/bmp",
  "image/gif",
  "image/jpeg",
  "image/pjpeg",
  "image/png",
  "image/svg+xml",
  "image/tiff",
  "image/webp",
  "image/x-icon",
];

const locationInput = document.querySelector(".location-input");
const viewInput = document.querySelector(".view-input");

const firstPreview = document.querySelector(".first-preview");
const secondPreview = document.querySelector(".second-preview");

const firstBlankImage = document.querySelector(".first-blank-img");
const secondBlankImage = document.querySelector(".second-blank-img");

locationInput.addEventListener("change", firstUpdateImageDisplay);
viewInput.addEventListener("change", secondUpdateImageDisplay);

function validFileType(file) {
  return fileTypes.includes(file.type);
}

function firstUpdateImageDisplay() {
  while (firstPreview.firstChild) {
    firstPreview.removeChild(firstPreview.firstChild);
  }

  const curFiles = locationInput.files;
  if (curFiles.length === 0) {
    const para = document.createElement("p");
    para.textContent = "선택한 파일을 업로드 할 수 없습니다.";
    firstPreview.appendChild(para);
  } else {
    const list = document.createElement("ol");
    firstPreview.appendChild(list);

    for (const file of curFiles) {
      const listItem = document.createElement("li");
      const para = document.createElement("p");

      if (validFileType(file)) {
        const image = document.createElement("img");
        image.src = URL.createObjectURL(file);
        listItem.appendChild(image);
        firstBlankImage.style.display = "none";
      } else {
        para.textContent = `File name ${file.name} : 파일의 형식이 옳지 않습니다.`;
        listItem.appendChild(para);
      }

      list.appendChild(listItem);
    }
  }
}

function secondUpdateImageDisplay() {
  while (secondPreview.firstChild) {
    secondPreview.removeChild(secondPreview.firstChild);
  }

  const curFiles = viewInput.files;
  if (curFiles.length === 0) {
    const para = document.createElement("p");
    para.textContent = "선택한 파일을 업로드 할 수 없습니다.";
    secondPreview.appendChild(para);
  } else {
    const list = document.createElement("ol");
    secondPreview.appendChild(list);

    for (const file of curFiles) {
      const listItem = document.createElement("li");
      const para = document.createElement("p");

      if (validFileType(file)) {
        const image = document.createElement("img");
        image.src = URL.createObjectURL(file);
        listItem.appendChild(image);
        secondBlankImage.style.display = "none";
      } else {
        para.textContent = `File name ${file.name} : 파일의 형식이 옳지 않습니다.`;
        listItem.appendChild(para);
      }

      list.appendChild(listItem);
    }
  }
}
