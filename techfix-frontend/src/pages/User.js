import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Row, Col, Card, Container, Modal } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/User.css';

const User = () => {
  const [users, setUsers] = useState([]);
  const [username, setUsername] = useState('');
  const [role, setRole] = useState('');
  const [password, setPassword] = useState('');
  const [editUser, setEditUser] = useState(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    fetchUsers();
  }, []);

  // Fetch all users from the backend
  const fetchUsers = async () => {
    try {
      const res = await axios.get('http://localhost:5000/api/user');
      setUsers(res.data);
    } catch (error) {
      console.error('Error fetching users:', error);
      toast.error('Error fetching users');
    }
  };

  // Handle user form submission for adding or editing a user
  const handleSubmit = async (e) => {
    e.preventDefault();

    const userData = {
      username,
      password,
      role,
    };

    try {
      if (editUser) {
        // Update existing user
        await axios.put(`http://localhost:5000/api/user/${editUser.userId}`, userData);
        toast.success('User updated successfully!');
      } else {
        // Add new user
        await axios.post('http://localhost:5000/api/user', userData);
        toast.success('User added successfully!');
      }
      fetchUsers();
      resetForm();
      handleModalClose();
    } catch (error) {
      console.error('Error submitting user:', error);
      toast.error('Error submitting user!');
    }
  };

  // Reset form fields
  const resetForm = () => {
    setUsername('');
    setRole('');
    setPassword('');
    setEditUser(null);
  };

  // Delete a user
  const handleDelete = async (userId) => {
    try {
      await axios.delete(`http://localhost:5000/api/user/${userId}`);
      fetchUsers();
      toast.success('User deleted successfully');
    } catch (error) {
      console.error('Error deleting user:', error);
      toast.error('Error deleting user');
    }
  };

  // Prepare to edit a user by setting form fields
  const handleEdit = (user) => {
    setEditUser(user);
    setUsername(user.username || '');
    setRole(user.role || '');
    setPassword(''); // Password is not loaded for editing
    setShowModal(true);
  };

  // Close the modal
  const handleModalClose = () => setShowModal(false);

  // Open modal for adding a new user
  const handleAddNewUser = () => {
    resetForm();
    setShowModal(true);
  };

  return (
    <Container className="user-container mt-5">
      <ToastContainer autoClose={5000} />
      <h1 className="text-center mb-4">Manage Users</h1>

      <div className="add-user-wrapper">
        <Button variant="primary" onClick={handleAddNewUser} className="mb-4">
          Add New User
        </Button>
      </div>

      <Row>
        {users.map((user) => (
          <Col md={4} key={user.userId} className="mb-4">
            <Card className="user-card shadow-sm">
              <Card.Body>
                <Card.Title>{user.username}</Card.Title>
                <Card.Text>
                  <strong>Role: </strong>{user.role}
                </Card.Text>
                <Button variant="warning" onClick={() => handleEdit(user)} className="me-2">
                  Edit
                </Button>
                <Button variant="danger" onClick={() => handleDelete(user.userId)}>
                  Delete
                </Button>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Modal for user form */}
      <Modal show={showModal} onHide={handleModalClose}>
        <Modal.Header closeButton>
          <Modal.Title>{editUser ? 'Update User' : 'Add User'}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="username" className="mb-3">
              <Form.Label>Username</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="role" className="mb-3">
              <Form.Label>Role</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter role (e.g., Admin, User)"
                value={role}
                onChange={(e) => setRole(e.target.value)}
                required
              />
            </Form.Group>

            {!editUser && (
              <Form.Group controlId="password" className="mb-3">
                <Form.Label>Password</Form.Label>
                <Form.Control
                  type="password"
                  placeholder="Enter password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />
              </Form.Group>
            )}

            <Button variant="primary" type="submit">
              {editUser ? 'Update User' : 'Add User'}
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
};

export default User;
