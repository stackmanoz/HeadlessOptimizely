import React from "react";
import Button from "react-bootstrap/Button";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import Form from "react-bootstrap/Form";

const NavigationMenu = ({ navigation }) => {
  return (
    <Navbar bg="dark" variant="dark">
      <Container>
        <Navbar.Brand href="/">
          <img
            src="https://getbootstrap.com/docs/5.2/assets/brand/bootstrap-logo.svg"
            alt="Bootstrap"
            width="30"
            height="24"
          ></img>
          SKETCHY{" "}
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="navbarScroll" />
        <Navbar.Collapse id="navbarScroll">
          <Nav
            className="me-auto my-2 my-lg-0"
            style={{ maxHeight: "100px" }}
            navbarScroll
          >
            {navigation &&
              navigation.map((item) => (
                // <Link to={`/common/${item.url}`} key={item.name}>
                //   {item.name}
                // </Link>
                <Nav.Link href={item.url} key={item.name}>
                  {item.name}
                </Nav.Link>
              ))}
          </Nav>
          <Form className="d-flex">
            <Form.Control
              type="search"
              placeholder="Search"
              className="me-2"
              aria-label="Search"
            />
            <Button variant="outline-success">Search</Button>
            <Nav.Link href="/login">Login</Nav.Link>
          </Form>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};
export default NavigationMenu;
