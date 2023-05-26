import React from 'react';

const NavigationMenu = ({ data }) => {
  console.log(data);
  const renderMenuItems = (items) => {
    return items.navigation && items.navigation.map((item) => (
      <li key={item.name}>
        <a href={item.url}>{item.name}</a>
        {item.child && item.child.length > 0 && (
          <ul>{renderSubMenuItems(item.child)}</ul>
        )}
      </li>
    ));
  };

  const renderSubMenuItems=(items)=>{
    return items.map((item)=>  (<li key={item.name}>
      <a href={item.url}>{item.name}</a>
      {item.child && item.child.length > 0 && (
        <ul>{renderSubMenuItems(item.child)}</ul>
      )}
    </li>))
  }

  return (<ul>{renderMenuItems(data)}</ul>);
};

export default NavigationMenu;