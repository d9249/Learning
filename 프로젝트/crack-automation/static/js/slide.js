const one = (ele) => document.querySelector(ele);
let pos = 0;

function next() {
  const wrap = one(".slide");
  const target = wrap.children[0];
  const len = target.children.length;
  target.style.cssText = `width:calc(100% * ${len}); display:flex; transition: 0.6s`;
  if (pos !== 2) {
    pos = (pos + 1) % len;
    target.style.marginLeft = `${-pos * 100}%`;
  }

  target.style.marginLeft = `${-pos * 100}%`;
}

function previous() {
  const wrap = one(".slide");
  const target = wrap.children[0];
  const len = target.children.length;
  target.style.cssText = `width:calc(100% * ${len}); display:flex; transition: 0.6s`;
  if (pos !== 0 && 2) {
    pos = (pos - 1) % len;
  }
  target.style.marginLeft = `${-pos * 100}%`;
}

window.onload = function () {
  const wrap = one(".slide");
  const target = wrap.children[0];
  const len = target.children.length;
  target.style.cssText = `width:calc(100% * ${len}); display:flex; transition: 0.6s`;
};
