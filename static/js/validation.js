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
const firstPreview = document.querySelector(".first-preview");
const firstBlankImage = document.querySelector(".first-blank-img");

const viewInput = document.querySelector(".view-input");
const secondPreview = document.querySelector(".second-preview");
const secondBlankImage = document.querySelector(".second-blank-img");

locationInput.addEventListener("change", () =>
  imageDisplay(locationInput, firstPreview, firstBlankImage)
);

viewInput.addEventListener("change", () =>
  imageDisplay(viewInput, secondPreview, secondBlankImage)
);

function validFileType(file) {
  return fileTypes.includes(file.type);
}

function imageDisplay(input, preview, blank) {
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
        blank.style.display = "none";
      } else {
        para.textContent = `File name ${file.name} : 파일의 형식이 옳지 않습니다.`;
        listItem.appendChild(para);
      }

      list.appendChild(listItem);
    }
  }
}
