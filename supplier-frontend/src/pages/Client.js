import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Row, Col, Card, Container, Modal } from 'react-bootstrap';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import '../styles/Client.css';

const Client = () => {
  const [clients, setClients] = useState([]);
  const [companyName, setCompanyName] = useState('');
  const [contactInfo, setContactInfo] = useState('');
  const [editClient, setEditClient] = useState(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    fetchClients();
  }, []);

  // Fetch all clients from the backend
  const fetchClients = async () => {
    try {
      const res = await axios.get('http://localhost:5001/api/client');
      setClients(res.data);
    } catch (error) {
      console.error('Error fetching clients:', error);
      toast.error('Error fetching clients');
    }
  };

  // Handle form submission for adding or editing a client
  const handleSubmit = async (e) => {
    e.preventDefault();

    const clientData = {
        companyName,
        contactInfo,
    };

    try {
        if (editClient) {
            // Ensure editClient has a valid clientId
            await axios.put(`http://localhost:5001/api/client/${editClient.clientId}`, clientData);
            toast.success('Client updated successfully!');
        } else {
            // Add new client
            await axios.post('http://localhost:5001/api/client', clientData);
            toast.success('Client added successfully!');
        }
        fetchClients();
        resetForm();
        handleModalClose();
    } catch (error) {
        console.error('Error submitting client:', error);
        toast.error('Error submitting client!');
    }
};

  // Reset form fields
  const resetForm = () => {
    setCompanyName('');
    setContactInfo('');
    setEditClient(null);
  };

  // Delete a client
  const handleDelete = async (clientId) => {
    try {
      await axios.delete(`http://localhost:5001/api/client/${clientId}`);
      fetchClients();
      toast.success('Client deleted successfully');
    } catch (error) {
      console.error('Error deleting client:', error);
      toast.error('Error deleting client');
    }
  };

  const handleEdit = (client) => {
    setEditClient(client); // Ensure client has clientId
    setCompanyName(client.companyName || '');
    setContactInfo(client.contactInfo || '');
    setShowModal(true);
};

  // Close the modal
  const handleModalClose = () => setShowModal(false);

  // Open modal for adding a new client
  const handleAddNewClient = () => {
    resetForm();
    setShowModal(true);
  };

  return (
    <Container className="client-container mt-5">
      <ToastContainer autoClose={5000} />
      <h1 className="text-center mb-4">Manage Clients</h1>

      <div className="add-client-wrapper">
        <Button variant="primary" onClick={handleAddNewClient} className="mb-4">
          Add New Client
        </Button>
      </div>

      <Row>
      {clients.map((client) => (
            <Col md={4} key={client.clientId} className="mb-4">
                {console.log(client.clientId)}
                <Card className="client-card shadow-sm">
                    <Card.Body>
                        <Card.Title>{client.companyName}</Card.Title>
                        <Card.Text>
                            <strong>Contact Info: </strong>{client.contactInfo}
                        </Card.Text>
                        <Button variant="warning" onClick={() => handleEdit(client)} className="me-2">
                            Edit
                        </Button>
                        <Button variant="danger" onClick={() => handleDelete(client.clientId)}>
                            Delete
                        </Button>
                    </Card.Body>
                </Card>
            </Col>
        ))}
      </Row>

      {/* Modal for client form */}
      <Modal show={showModal} onHide={handleModalClose}>
        <Modal.Header closeButton>
          <Modal.Title>{editClient ? 'Update Client' : 'Add Client'}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="companyName" className="mb-3">
              <Form.Label>Company Name</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter company name"
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="contactInfo" className="mb-3">
              <Form.Label>Contact Info</Form.Label>
              <Form.Control
                type="text"
                placeholder="Enter contact info"
                value={contactInfo}
                onChange={(e) => setContactInfo(e.target.value)}
                required
              />
            </Form.Group>

            <Button variant="primary" type="submit">
              {editClient ? 'Update Client' : 'Add Client'}
            </Button>
          </Form>
        </Modal.Body>
      </Modal>
    </Container>
  );
};

export default Client;
