import React, { useEffect, useState } from 'react';
import { Navbar, Nav, Button } from 'react-bootstrap';
import { Link, useNavigate } from 'react-router-dom';
import '../styles/NavBar.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const NavBar = () => {
  const navigate = useNavigate();
  const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('authToken'));

  // Check the authentication status on component mount
  useEffect(() => {
    const checkAuth = () => {
      setIsAuthenticated(!!localStorage.getItem('authToken'));
      console.log('Is Authenticated: ', !!localStorage.getItem('authToken'));
    };

    // Listen to changes in localStorage (in case login happens in another tab)
    window.addEventListener('storage', checkAuth);

    return () => {
      window.removeEventListener('storage', checkAuth);
    };
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('authToken'); // Clear token on logout
    setIsAuthenticated(false); // Update the state manually
    navigate('/');
  };

  return (
    <Navbar bg="light" expand="lg" className="navbar-custom">
      <Navbar.Brand href="/" className="navbar-logo">
        Supplier Management
      </Navbar.Brand>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="me-auto">
          {isAuthenticated ? (
            <>
              <Nav.Link as={Link} to="/product">Products</Nav.Link>
              <Nav.Link as={Link} to="/rfq">RFQs</Nav.Link>
              <Nav.Link as={Link} to="/client">Clients</Nav.Link>
              <Nav.Link as={Link} to="/user">Users</Nav.Link>
            </>
          ) : (
            <Nav.Link as={Link} to="/">Login</Nav.Link>
          )}
        </Nav>
        {isAuthenticated ? (
          <div className="d-flex">
            <Button variant="outline-danger" onClick={handleLogout}>Logout</Button>
          </div>
        ) : (
          <Button variant="outline-success" onClick={() => navigate('/')}>Login</Button>
        )}
      </Navbar.Collapse>
    </Navbar>
  );
};

export default NavBar;
