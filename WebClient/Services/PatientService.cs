using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Repository;
using ClassLibrary.Domain;
using ClassLibrary.Service;
using static ClassLibrary.Proxy.LogInProxy;
using User = ClassLibrary.Domain.User;
using System.Net;
using ClassLibrary.Model;

namespace WebClient.Services
{
    class PatientService : IPatientService
    {
        private readonly IPatientRepository _patient_repository;
        private readonly ILoggerService _logger_service;

        public PatientJointModel patientInfo { get; private set; } = PatientJointModel.Default;
        public List<PatientJointModel> patientList { get; private set; } = new List<PatientJointModel>();

        public PatientService(IPatientRepository patient_repository, ILoggerService logger_service)
        {
            this._patient_repository = patient_repository;

            this._logger_service = logger_service ?? new LoggerService(new ClassLibrary.Proxy.LoggerProxy());
        }

        public async Task<bool> loadPatientInfoByUserId(int user_id)
        {
            try
            {
                Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(user_id);
                List<User> domain_users = await this._patient_repository.getAllUserAsync();
                User filtered_user = domain_users.Find(user => user.userId == user_id);

                if (domain_patient == null || filtered_user == null)
                {
                    patientInfo = PatientJointModel.Default;
                    return false;
                }

                this.patientInfo = mapToJointModel(domain_patient, filtered_user);
                return true;
            }
            catch (Exception ex)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }
        }

        public async Task<bool> loadAllPatients()
        {
            List<Patient> domain_patients = await this._patient_repository.getAllPatientsAsync();
            List<User> domain_users = await this._patient_repository.getAllUserAsync();

            this.patientList.Clear();
            foreach (var patient in domain_patients)
            {
                User? matched_user = domain_users.Find(user => user.userId == patient.userId);
                if (matched_user != null)
                {
                    this.patientList.Add(mapToJointModel(patient, matched_user));
                }
            }
            return true;
        }

        public virtual async Task<bool> updatePassword(int user_id, string _password)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.password = _password;
            await this._patient_repository.updatePatientAsync(filtered_user.userId, domain_patient);

            return true;
        }

        public virtual async Task<bool> updateName(int user_id, string name)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.name = name;
            await (_patient_repository as WebClient.Proxy.PatientProxy).updateUserAsync(user_id, filtered_user);

            return true;
        }

        public virtual async Task<bool> updateAddress(int user_id, string address)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.address = address;
            await (_patient_repository as WebClient.Proxy.PatientProxy).updateUserAsync(user_id, filtered_user);

            return true;
        }

        public virtual async Task<bool> updatePhoneNumber(int user_id, string phone_number)
        {
            Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
            List<User> domain_users = await this._patient_repository.getAllUserAsync();
            User filtered_user = domain_users.Find(user => user.userId == user_id);
            if (domain_patient == null || filtered_user == null)
            {
                patientInfo = PatientJointModel.Default;
                return false;
            }

            filtered_user.phoneNumber = phone_number;
            await (_patient_repository as WebClient.Proxy.PatientProxy).updateUserAsync(user_id, filtered_user);

            return true;
        }

        public virtual async Task<bool> updateEmergencyContact(int user_id, string emergency_contact)
        {
            try
            {
                Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
                List<User> domain_users = await this._patient_repository.getAllUserAsync();
                User filtered_user = domain_users.Find(user => user.userId == user_id);
                if (domain_patient == null || filtered_user == null) return false;

                domain_patient.EmergencyContact = emergency_contact;
                await this._patient_repository.updatePatientAsync(filtered_user.userId, domain_patient);

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error updating emergency contact: {exception.Message}");
                return false;
            }
        }

        public virtual async Task<bool> updateWeight(int user_id, double weight)
        {
            try
            {
                Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(user_id);
                List<User> domain_users = await this._patient_repository.getAllUserAsync();
                User filtered_user = domain_users.Find(user => user.userId == user_id);
                if (domain_patient == null || filtered_user == null) return false;
                
                domain_patient.weight = weight;
                await this._patient_repository.updatePatientAsync(filtered_user.userId, domain_patient); 

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error updating weight: {exception.Message}");
                return false;
            }
        }

        public virtual async Task<bool> updateHeight(int user_id, int height)
        {
            try
            {
                Patient domain_patient = await this._patient_repository.getPatientByUserIdAsync(user_id);
                List<User> domain_users = await this._patient_repository.getAllUserAsync();
                User filtered_user = domain_users.Find(user => user.userId == user_id);
                if (domain_patient == null || filtered_user == null) return false;

                domain_patient.height = height;
                await this._patient_repository.updatePatientAsync(filtered_user.userId, domain_patient);

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error updating height: {exception.Message}");
                return false;
            }
        }

        public virtual async Task<bool> updateBloodType(int user_id, string blood_type)
        {
            try
            {
                Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
                List<User> domain_users = await this._patient_repository.getAllUserAsync();
                User filtered_user = domain_users.Find(user => user.userId == user_id);
                if (domain_patient == null || filtered_user == null) return false;
                domain_patient.bloodType = blood_type;
                await this._patient_repository.updatePatientAsync(filtered_user.userId, domain_patient);

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error updating blood type: {exception.Message}");
                return false;
            }
        }

        public virtual async Task<bool> updateAllergies(int user_id, string allergies)
        {
            try
            {
                Patient domain_patient = await _patient_repository.getPatientByUserIdAsync(user_id);
                List<User> domain_users = await this._patient_repository.getAllUserAsync();
                User filtered_user = domain_users.Find(user => user.userId == user_id);
                if (domain_patient == null || filtered_user == null) return false;
                domain_patient.allergies = allergies;
                await this._patient_repository.updatePatientAsync(filtered_user.userId, domain_patient);
                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error updating allergies: {exception.Message}");
                return false;
            }
        }

        public virtual async Task<bool> logUpdate(int user_id, ActionType action)
        {
            try
            {
                return await _logger_service.logAction(user_id, action);
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error logging update: {exception.Message}");
                return false;
            }
        }

        private PatientJointModel mapToJointModel(Patient domain_patient, User domain_user)
        {
            return new PatientJointModel(
                domain_patient.userId,
                domain_user.userId,
                domain_user.name,
                domain_patient.bloodType ?? string.Empty,
                domain_patient.EmergencyContact ?? string.Empty,
                domain_patient.allergies ?? string.Empty,
                domain_patient.weight,
                domain_patient.height,
                domain_user.username,
                domain_user.password,
                domain_user.mail,
                domain_user.birthDate,
                domain_user.cnp ?? string.Empty,
                domain_user.address ?? string.Empty,
                domain_user.phoneNumber ?? string.Empty,
                domain_user.registrationDate
            );
        }
    }
}
