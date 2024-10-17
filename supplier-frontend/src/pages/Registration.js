import React, { useState } from 'react';
import { Form, Button, Container, Row, Col } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import axios from "axios";
import '../styles/Registration.css'; // Assume custom styles here

const RegistrationPage = () => {
  const navigate = useNavigate();
  const [registrationInfo, setRegistrationInfo] = useState({
    username: "",
    password: "",
    confirmPassword: "",
    role: "customer",
  });
  const [error, setError] = useState('');

  const handleChange = (evt) => {
    setRegistrationInfo({ ...registrationInfo, [evt.target.name]: evt.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (registrationInfo.password !== registrationInfo.confirmPassword) {
      setError("Passwords do not match");
      toast.error("Passwords do not match");
      return;
    }

    try {
      const response = await axios.post("http://localhost:5001/api/auth/register", registrationInfo);
      toast.success("Registration successful! Please login.");
      navigate('/');
    } catch (error) {
      setError("Registration failed");
      toast.error("Registration failed. Please try again.");
    }
  };

  return (
    <>
      <ToastContainer position="top-center" />
      <Container className="registration-container">
        <Row className="justify-content-md-center registration-page">
          <Col xs={12} md={6} lg={4} className="registration-form">
            <h2 className="text-center">Register</h2>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            <Form onSubmit={handleSubmit}>
              <Form.Group controlId="formBasicUsername">
                <Form.Control
                  type="text"
                  name="username"
                  value={registrationInfo.username}
                  onChange={handleChange}
                  placeholder="Enter username"
                  required
                />
              </Form.Group>

              <Form.Group controlId="formBasicPassword" className="mt-3">
                <Form.Control
                  type="password"
                  name="password"
                  value={registrationInfo.password}
                  onChange={handleChange}
                  placeholder="Password"
                  required
                />
              </Form.Group>

              <Form.Group controlId="formBasicConfirmPassword" className="mt-3">
                <Form.Control
                  type="password"
                  name="confirmPassword"
                  value={registrationInfo.confirmPassword}
                  onChange={handleChange}
                  placeholder="Confirm Password"
                  required
                />
              </Form.Group>

              <Form.Group controlId="formBasicRole" className="mt-3">
                <Form.Label>Select Role</Form.Label>
                <Form.Control
                  as="select"
                  name="role"
                  value={registrationInfo.role}
                  onChange={handleChange}
                >
                  <option value="customer">Customer</option>
                  <option value="staff">Staff</option>
                  <option value="admin">Admin</option>
                </Form.Control>
              </Form.Group>

              <Button variant="primary" type="submit" className="mt-3">
                Register
              </Button>

              <div className="mt-3 text-center">
                <p>Already have an account? <a href="/">Login here</a></p>
              </div>
            </Form>
          </Col>
        </Row>
      </Container>
    </>
  );
};

export default RegistrationPage;
