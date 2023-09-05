import React, { useState, useEffect, useContext } from "react";
import NavigationMenu from "./NavigationMenu";
import ComponentRenderer from "./ComponentRenderer";
import Footer from "./Footer/Footer";
import { constants } from "../AppUrls";
import { StateContext } from "../StateContext";

const Layout = ({ children }) => {
  const [navigation, setNavigation] = useState([]);
  const [pageData, setPageData] = useState({});
  const { component, setStateData } = useContext(StateContext);

  const setCookie = (cname, cvalue, exdays) => {
    const d = new Date();
    d.setTime(d.getTime() + exdays * 24 * 60 * 60 * 1000);
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
  };

  useEffect(() => {
    const fetchData = () => {
      const requestOptions = {
        method: "GET",
        credentials: "same-origin",
        headers: {
          "Content-Type": "application/json",
          AnonymousId: getCookie("AnonymousId"), // Set the cookie in the request headers
        },
      };

      fetch(constants.BASE_URL + window.location.pathname, requestOptions)
        .then((response) => response.json())
        .then((data) => {
          setNavigation(data.commerceNavigation);
          setPageData(data.currentContent);
          setStateData(data.currentContent);
          setCookie("AnonymousId", data.userId, 10);
        })
        .catch((error) => console.error(error));
    };
    fetchData();
  }, []);

  const getCookie = (cname) => {
    console.log(cname);
    const name = cname + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookieArray = decodedCookie.split(";");
    for (let i = 0; i < cookieArray.length; i++) {
      let cookie = cookieArray[i];
      while (cookie.charAt(0) === " ") {
        cookie = cookie.substring(1);
      }
      if (cookie.indexOf(name) === 0) {
        return cookie.substring(name.length, cookie.length);
      }
    }
    return "";
  };

  return (
    <div className="">
      <header>
        <NavigationMenu navigation={navigation} />
      </header>
      <main className="container" style={{ marginTop: "50px" }}>
        <ComponentRenderer pageData={pageData} />
        {children}
      </main>
      <Footer />
    </div>
  );
};

export default Layout;
