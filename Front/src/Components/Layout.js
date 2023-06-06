import React, { useState, useEffect } from "react";
import NavigationMenu from "./NavigationMenu";
import { constants } from "../AppUrls";

const Layout = ({ children }) => {
  const [data, setData] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(
          constants.BASE_URL + window.location.pathname
        );
        const jsonData = await response.json();
        console.log(jsonData);
        setData(jsonData);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchData();
  }, []);

  return (
    <div className="container-fluid">
      <header>
        <NavigationMenu data={data} />
      </header>
      <main>{children}</main>
      <footer></footer>
    </div>
  );
};

export default Layout;
