import React, { useContext } from "react";
import { Container, Row, Col, Button, Badge, ListGroup } from "react-bootstrap";
import { StateContext } from "../../StateContext";

const Product = () => {
  const { component, setStateData } = useContext(StateContext);

  const src = "https://localhost:5002/globalassets/productimages";

  const setVariant = (variant) => {
    setStateData((prevState) => ({
      ...prevState,
      code: variant.code,
    }));
  };

  async function addToCart() {
    var urlencoded = new URLSearchParams();
    urlencoded.append("code", component.code);
    urlencoded.append("quantity", 1);

    var requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
        AnonymousId: getCookie("AnonymousId"), // Set the cookie in the request headers
      },
      body: urlencoded.toString(), // Convert the URLSearchParams object to a string
      redirect: "follow",
    };

    try {
      const response = await fetch(
        "https://localhost:5002/api/cart/addtocart",
        requestOptions
      );
      const result = await response.json();
      // Handle the response data
    } catch (error) {
      console.log("error", error);
    }
  }

  async function payAsYouGo() {
    var requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
        AnonymousId: getCookie("AnonymousId"), // Set the cookie in the request headers
      },
      redirect: "follow",
    };

    try {
      const response = await fetch(
        "https://localhost:5002/klarnacheckout/ProcessOrderPurchaseOrder",
        requestOptions
      );
      const result = await response.json();

      document.open("https://localhost:5002/", true);
      document.write(result.htmlSnippet);
      document.close();
      // Handle the response data
    } catch (error) {
      console.log("error", error);
    }
  }

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
    <Container>
      <Row>
        <Col md={6}>
          <img
            src={`${src}/${component.productMainImage}`}
            alt="Product"
            className="img-fluid"
          />
          <Row>
            {component.images.map((item) => (
              <Col md={3} key={item.imageName}>
                <img
                  src={`https://localhost:5002${item.imageUrl}`}
                  alt="Product"
                  className="img-fluid"
                />
              </Col>
            ))}
          </Row>
        </Col>
        <Col md={6}>
          <h1>{component.productName}</h1>
          {component.variants.map((v) => (
            <div>
              <a
                href="javascript:void(0)"
                key={v.code}
                onClick={() => setVariant(v)} // Pass the variant code to the setVariant function
              >
                {v.variantName}
              </a>
              <button type="button" class="btn btn-link">
                {v.size}
              </button>
            </div>
          ))}
          <hr />
          <h3>Price: ${component.price}</h3>
          <p>Description of the product goes here.</p>
          <Button variant="primary" onClick={() => addToCart()}>
            Add to Cart
          </Button>{" "}
          <Button variant="warning">Got to Checkout</Button>{" "}
          <Button variant="warning" onClick={() => payAsYouGo()}>
            Go to Payment
          </Button>
          <hr />
          <h4>Product Details</h4>
          <ListGroup>
            <ListGroup.Item>
              <strong>Brand:</strong> Brand Name
            </ListGroup.Item>
            <ListGroup.Item>
              <strong>Color:</strong> Black
            </ListGroup.Item>
            <ListGroup.Item>
              <strong>Size:</strong> Large
            </ListGroup.Item>
            <ListGroup.Item>
              <strong>Material:</strong> Cotton
            </ListGroup.Item>
          </ListGroup>
        </Col>
      </Row>
    </Container>
  );
};

export default Product;
