import React from "react";
import { Container, Row, Col, Button, Card } from "react-bootstrap";

const Home = () => {
  return (
    <Container>
      <Row className="mt-5">
        <Col md={6}>
          <h1>LOREM IPSUM</h1>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut
            tristique, urna ut lobortis euismod, neque mi ullamcorper nisi, vel
            placerat tortor mauris a nulla.Lorem ipsum dolor sit amet,
            consectetur adipiscing elit. Ut tristique, urna ut lobortis euismod,
            neque mi ullamcorper nisi, vel placerat tortor mauris a nulla.Lorem
            ipsum dolor sit amet, consectetur adipiscing elit. Ut tristique,
            urna ut lobortis euismod, neque mi ullamcorper nisi, vel placerat
            tortor mauris a nulla.Lorem ipsum dolor sit amet, consectetur
            adipiscing elit. Ut tristique, urna ut lobortis euismod, neque mi
            ullamcorper nisi, vel placerat tortor mauris a nulla.
          </p>
          <Button variant="primary">Learn More</Button>
        </Col>
        <Col md={6}>
          <img
            src="https://picsum.photos/800/400"
            alt="Home"
            className="img-fluid"
          />
        </Col>
      </Row>

      <Row className="mt-5">
        <Col md={4}>
          <Card>
            <Card.Img variant="top" src="https://via.placeholder.com/300x200" />
            <Card.Body>
              <Card.Title>Feature 1</Card.Title>
              <Card.Text>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut
                tristique, urna ut lobortis euismod.
              </Card.Text>
              <Button variant="primary">Read More</Button>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card>
            <Card.Img variant="top" src="https://via.placeholder.com/300x200" />
            <Card.Body>
              <Card.Title>Feature 2</Card.Title>
              <Card.Text>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut
                tristique, urna ut lobortis euismod.
              </Card.Text>
              <Button variant="primary">Read More</Button>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card>
            <Card.Img variant="top" src="https://via.placeholder.com/300x200" />
            <Card.Body>
              <Card.Title>Feature 3</Card.Title>
              <Card.Text>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut
                tristique, urna ut lobortis euismod.
              </Card.Text>
              <Button variant="primary">Read More</Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default Home;
