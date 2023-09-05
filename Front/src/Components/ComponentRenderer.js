import React from "react";
import Category from "./Category/Category";
import Product from "./Product/Product";

const ComponentRenderer = ({ pageData }) => {
  if (!pageData && !pageData.typeName) return;
  let component = <h1>No Page Found</h1>;
  switch (pageData.typeName) {
    case "Category":
      component = <Category content={pageData} />;
      break;
    case "Product":
      component = <Product data={pageData} />;
      break;
    default:
      component = <div />;
      break;
  }
  return component;
};

export default ComponentRenderer;
