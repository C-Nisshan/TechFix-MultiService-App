import React, { useState } from 'react';
import { Form, Button, Container, Row, Col } from "react-bootstrap";
import { useNavigate, Link } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import axios from "axios";
import { useCookies } from 'react-cookie';
import "../styles/Login.css"; // Assume custom styles here

const LoginPage = () => {
  const navigate = useNavigate();
  const [loginInfo, setLoginInfo] = useState({
    username: "",
    password: "",
  });
  const [cookies, setCookie] = useCookies(['authToken']);
  const [error, setError] = useState('');

  const handleChange = (evt) => {
    setLoginInfo({ ...loginInfo, [evt.target.name]: evt.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post("http://localhost:5001/api/auth/login", loginInfo);
      const { token } = response.data;

      if (token) {
        // Save token in localStorage and cookies
        localStorage.setItem('authToken', token);
        setCookie('token', token, { path: '/' });

        toast.success("Login successful!");
        navigate('/product');
      }
    } catch (error) {
      if (error.response && error.response.data && error.response.data.msg) {
        setError(error.response.data.msg);
        toast.error(error.response.data.msg);
      } else {
        setError("Invalid username or password");
        toast.error("An error occurred. Please try again.");
      }
    }
  };

  return (
    <>
      <ToastContainer position="top-center" />
      <Container className="login-container">
        <Row className="justify-content-md-center login-page">
          <Col xs={12} md={6} lg={4} className="login-form">
            <h2 className="text-center">Login</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <Form onSubmit={handleSubmit}>
              <Form.Group controlId="formBasicUsername">
                <Form.Control
                  type="text"
                  name="username"
                  value={loginInfo.username}
                  onChange={handleChange}
                  placeholder="Enter username"
                  required
                />
              </Form.Group>

              <Form.Group controlId="formBasicPassword" className="mt-3">
                <Form.Control
                  type="password"
                  name="password"
                  value={loginInfo.password}
                  onChange={handleChange}
                  placeholder="Password"
                  required
                />
              </Form.Group>

              <div className="mt-3 forgot_pass">
                <a href="#">Forgot password?</a>
              </div>

              <Button variant="primary" type="submit" className="mt-3">
                Login
              </Button>

              <div className="mt-3 text-center">
                <p>Don't have an account? <Link to="/registration">Register Here</Link></p>
              </div>
            </Form>
          </Col>
        </Row>
      </Container>
    </>
  );
};

export default LoginPage;
