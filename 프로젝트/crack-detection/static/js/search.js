const searchField = document.querySelector("#searchField");
const cardOutput = document.querySelector(".card-output");
const card = document.querySelector(".card-deck");
const page = document.querySelector(".page");
const result = document.querySelector(".result");
cardOutput.style.display = "none";

searchField.addEventListener("keyup", (event) => {
  const searchValue = event.target.value;
  if (searchValue.trim().length > 0) {
    page.style.disply = "none";
    cardOutput.innerHTML = "";
    fetch("/search", {
      body: JSON.stringify({ searchText: searchValue }),
      method: "POST",
    })
      .then((res) => res.json())
      .then((data) => {
        card.style.display = "none";
        cardOutput.style.display = "flex";

        if (data.length === 0) {
          result.style.display = "block";
          cardOutput.style.display = "none";
        } else {
          result.style.display = "none";
          data.map((item) => {
            if (item.isFlattened == true) {
              item.isFlattened = "O";
            } else {
              item.isFlattened = "X";
            }

            cardOutput.innerHTML += `
              <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="card">    
                  <div class="view overlay">
                    <img class="card-img-top" src="/media/${item.image}" alt="Card image cap">
                  </div>
                  <div class="card-body">
                    <h4 class="card-title">카테고리 : ${item.category_name}</h4>
                    <p class="card-text"><p>현황 : ${item.state}</p>
                    <p class="card-text"><p>원인 : ${item.cause}</p>
                    <p class="card-text"><p>해결방안 : ${item.solution}</p>
                    <p class="card-text"><p>평탄화 : ${item.isFlattened}</p>
                    <a href="/db/${item.id}" class="btn btn-primary">더보기</a>
                  </div>
                </div>
              </div>
            `;
          });
        }
      });
  } else {
    cardOutput.style.display = "none";
    card.style.display = "flex";
    page.style.disply = "block";
    result.style.display = "none";
  }
});
