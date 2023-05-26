import React, { useState, useEffect } from 'react';
import NavigationMenu from './Components/NavigationMenu';

function App() {
    const [data, setData] = useState([]);
    const porturl = "https://localhost:5000";

    useEffect(() => {
      const fetchData = async () => {
        try {
          const response = await fetch(porturl + window.location.pathname);
          const jsonData = await response.json();
          setData(jsonData);
        } catch (error) {
          console.error('Error fetching data:', error);
        }
      };
  
      fetchData();
    }, []);

    return (
        <div>
            <h1>Navigation Menu</h1>
            <NavigationMenu data={data} />
        </div>
      );
}

export default App;