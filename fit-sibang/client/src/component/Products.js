import React from "react"
import "../css/Products.css"
import TopHeader from "./TopHeader"
import { ProductData } from "./ProductData"
import Footer from "./Footer"

export default function Products(props) {
  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <TopHeader />
      {/* 반복문을 통해 ProductData.js의 내용들을 출력 */}
      {ProductData.map((item, index) => {
        return (
          <div key={index} className={item.cName}>
            <div className="wrapper">
              <div className="card">
                <div className="product-left">
                  <div className="header">
                    <h2>
                      {item.number}, {item.title}
                    </h2>
                  </div>
                  <div className="product-main">
                    <p>{item.explanation}</p>
                  </div>
                  <div className="product-details">
                    <div className="product-total">
                      <h3>{item.price}</h3>
                    </div>
                  </div>
                </div>
                <div className="product-right">
                  <img src={item.image} alt="err" />
                </div>
              </div>
            </div>
          </div>
        )
      })}
      <Footer />
    </div>
  )
}
