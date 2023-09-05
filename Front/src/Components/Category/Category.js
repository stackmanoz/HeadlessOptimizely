import React from "react";
import Card from "react-bootstrap/Card";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";

const Category = ({ content }) => {
  const src = "https://localhost:5002/globalassets/productimages";

  return (
    <div className="d-flex justify-content-around">
      <Row>
        {content.products.map((item) => (
          <Col sm={3} key={item.productName}>
            <Card style={{ width: "18rem" }}>
              <Card.Img variant="top" src={`${src}/${item.productMainImage}`} />
              <Card.Body>
                <Card.Title>{item.productName}</Card.Title>
                <Card.Text>
                  Some quick example text to build on the card title and make up
                  the bulk of the card's content.
                </Card.Text>
              </Card.Body>
              <Card.Footer>
                <Row className="d-flex align-items-center">
                  <Col className="text-start col-4">${item.price}</Col>
                  <Col className="text-end col-8">
                    <a className="btn btn-primary" href={item.url}>
                      View Product
                    </a>
                  </Col>
                </Row>
              </Card.Footer>
            </Card>
          </Col>
        ))}
      </Row>
    </div>
  );
};

export default Category;
