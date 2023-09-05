import React from "react";
import { Col, Button, Row, Container, Card, Form } from "react-bootstrap";

import { constants } from "../../AppUrls";

class LoginComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      email: "",
      password: "",
      rememberMe: true,
    };
  }

  submitForm(e) {
    e.preventDefault();
    this.postData(constants.BASE_URL + "/api/account/login", {
      email: this.state.email,
      password: this.state.password,
      isPersistent: this.state.rememberMe,
    }).then((data) => {
      console.log(data); // JSON data parsed by `data.json()` call
    });
  }

  async postData(url = "", data = {}) {
    var urlencoded = new URLSearchParams();
    urlencoded.append("email", data.email);
    urlencoded.append("password", data.password);
    urlencoded.append("isPersistent", data.isPersistent);
    urlencoded.append("redirectUrl", data.isPersistent);

    var requestOptions = {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      body: urlencoded,
      redirect: "follow",
    };

    try {
      const response = await fetch(
        constants.BASE_URL + "/api/account/login",
        requestOptions
      );
      const result = await response.json();
      if (result.statusCode === 200 && result.redirectUrl) {
        window.location.replace("/");
      }
    } catch (error) {
      return console.log("error", error);
    }
  }

  handleInputChange(event) {
    const target = event.target;
    const name = target.name;
    const value = target.value;

    this.setState({
      [name]: value,
    });
  }

  handleChecked() {
    this.setState(
      {
        rememberMe: !this.state.rememberMe, // flip boolean value
      },
      () => {
        console.log(this.state);
      }
    );
  }

  render() {
    return (
      <div>
        <Container>
          <Row className="vh-100 d-flex justify-content-center align-items-center">
            <Col md={8} lg={6} xs={12}>
              <div className="border border-3 border-primary"></div>
              <Card className="shadow">
                <Card.Body>
                  <div className="mb-3 mt-md-4">
                    <h2 className="fw-bold mb-2 text-uppercase ">
                      AN SKETCHY LOGIN
                    </h2>
                    <p className=" mb-5">
                      Please enter your login and password!
                    </p>
                    <div className="mb-3">
                      <Form onSubmit={this.submitForm.bind(this)}>
                        <Form.Group className="mb-3" controlId="formBasicEmail">
                          <Form.Label className="text-center">
                            Email address
                          </Form.Label>
                          <Form.Control
                            type="email"
                            name="email"
                            placeholder="Enter email"
                            value={this.state.email}
                            onChange={this.handleInputChange.bind(this)}
                          />
                        </Form.Group>

                        <Form.Group
                          className="mb-3"
                          controlId="formBasicPassword"
                        >
                          <Form.Label>Password</Form.Label>
                          <Form.Control
                            type="password"
                            name="password"
                            placeholder="Password"
                            value={this.state.password}
                            onChange={this.handleInputChange.bind(this)}
                          />
                        </Form.Group>
                        <Form.Group
                          className="mb-3"
                          controlId="formBasicCheckbox"
                        >
                          <Form.Check
                            name="rememberMe"
                            checked={this.state.rememberMe}
                            onChange={this.handleChecked.bind(this)}
                            id="flexCheckDefault"
                            label="Remember me"
                          />
                          <p className="small">
                            <a className="text-primary" href="#!">
                              Forgot password?
                            </a>
                          </p>
                        </Form.Group>
                        <div className="d-grid">
                          <Button variant="primary" type="submit">
                            Login
                          </Button>
                        </div>
                      </Form>
                      <div className="mt-3">
                        <p className="mb-0  text-center">
                          Don't have an account?{" "}
                          <a href="{''}" className="text-primary fw-bold">
                            Sign Up
                          </a>
                        </p>
                      </div>
                    </div>
                  </div>
                </Card.Body>
              </Card>
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}

export default LoginComponent;
