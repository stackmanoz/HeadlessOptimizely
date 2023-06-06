import React, { useState, useEffect } from "react";
import LoginComponent from "./Components/Account/Login";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Layout from "./Components/Layout";

function App() {
  const [data, setData] = useState([]);

  useEffect(() => {}, []);

  return (
    <div>
      <Layout children={data} />
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginComponent />}></Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
