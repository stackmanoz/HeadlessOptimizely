import React from "react";
import { Container, Row, Col } from "react-bootstrap";

const Footer = () => {
  return (
    <footer className="bg-dark text-light" style={{ marginTop: "50px" }}>
      <Container style={{ paddingTop: "20px" }}>
        <Row>
          <Col md={4}>
            <h5>About Us</h5>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>
          </Col>
          <Col md={4}>
            <h5>Contact Us</h5>
            <p>Email: info@example.com</p>
            <p>Phone: 123-456-7890</p>
          </Col>
          <Col md={4}>
            <h5>Follow Us</h5>
            <p>Facebook</p>
            <p>Twitter</p>
          </Col>
        </Row>
      </Container>
    </footer>
  );
};

export default Footer;
