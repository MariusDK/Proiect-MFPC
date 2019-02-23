using MySql.Data.MySqlClient;
using ProiectMFPC_V1.src.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProiectMFPC_V1.src
{
    class Transaction
    {
        public int id;
        public DateTime timestamp;
        public string status;
        public List<string> operations;
        private Operation operationObj;
        private LockTable lockTable;
        private List<string> logTable;
        private DeadlockDetector detector;
        private ProviderTransaction providerTransaction;
        public string operationFormatValue;
        public string response="";

        public Transaction()
        {
            operationObj = new Operation();
            lockTable = new LockTable();
            detector = new DeadlockDetector();
            logTable = new List<string>();
            providerTransaction = new ProviderTransaction();
            this.id = providerTransaction.GetNextIdTransacion();
            this.status = "active";
        }
        public Transaction(int id,List<string> operations)
        {
            this.id = id;
            this.timestamp = DateTime.Now;
            this.status = "active";
            this.operations = operations;
            operationObj = new Operation();
            lockTable = new LockTable();
            detector= new DeadlockDetector();
            logTable = new List<string>();
        }
        public Transaction(int id,DateTime timestamp, string status, string operations)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.status = status;
            this.operationFormatValue = operations;
        }
        public void Run()
        {
            //Console.WriteLine("Aici");
            Transaction transaction = providerTransaction.GetTransaction(id);
            for(int i=0;i<operations.Count;i++)
            {
                if (transaction.status.Equals("aboard"))
                {
                    Console.WriteLine("Tranzactia "+id+" reporneste!");
                    i = 0;
                    logTable = new List<string>();
                    transaction.status = "active";
                    providerTransaction.UpdateTransaction(transaction);
                }
                string[] operationForm = operations[i].Split(";");
                string tableName = GetTableName(operationForm[0]);
                int resurceId = 0;
                if (operationForm[1].Equals("null"))
                {
                    if (operationForm[0].Contains("Student"))
                    {
                        resurceId = operationObj.GetNextIdStudent();
                    }
                    if (operationForm[0].Contains("Profesor"))
                    {
                        resurceId = operationObj.GetNextIdProfesor();
                    }
                    if (operationForm[0].Contains("Curs"))
                    {
                        resurceId = operationObj.GetNextIdCurs();
                    }
                }
                else
                {
                    resurceId = GetResourceId(operationForm[1]);
                }
                string lockType = GetLockType(operationForm[0]);
                List<int> transWaitLock = new List<int>();
                int idLock = lockTable.IsLock(resurceId, tableName);
                while (true)
                {
                    idLock = lockTable.IsLock(resurceId, tableName);
                    if (idLock == 0)
                    {
                        idLock = lockTable.ObtainLock(lockType, tableName, resurceId, id);
                        int idDeadlock = detector.AddDataToDeadlockTable(lockType, tableName, resurceId, id, transWaitLock);
                        if (operationForm[0].Contains("Get"))
                        {
                            Console.WriteLine("Tranzactia " + id + " a obtinut lock-ul pe " + operationForm[0]);
                            string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + null;
                            logTable.Add(val);
                            break;
                        }
                        if (operationForm[0].Contains("Insert"))
                        {
                            Console.WriteLine("Tranzactia " + id + " a obtinut lock-ul pe " + operationForm[0]);
                            string val = operationForm[0] + ";" + null + ";" + null + ";" + operationForm[2];
                            logTable.Add(val);
                            break;
                        }
                        if (operationForm[0].Contains("Update"))
                        {
                            //valoarea veche
                            Console.WriteLine("Tranzactia " + id + " a obtinut lock-ul pe " + operationForm[0]);
                            string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + operationForm[2];
                            logTable.Add(val);
                            break;
                        }
                        if (operationForm[0].Contains("Delete"))
                        {
                            //valoarea veche
                            Console.WriteLine("Tranzactia " + id + " a obtinut lock-ul pe " + operationForm[0]);
                            string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + null;
                            logTable.Add(val);
                            break;
                        }
                    }
                    else
                    {
                        Lock currentLock = lockTable.GetLock(idLock);
                        Deadlock deadlock = detector.GetDeadlock(resurceId, tableName);
                        if (currentLock != null)
                        {
                            if ((currentLock.transactionId == id))
                            {
                                if (operationForm[0].Contains("Get"))
                                {
                                    Console.WriteLine("Tranzactia " + id + " a modificat lock-ul pe " + operationForm[0]);
                                    string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + null;
                                    logTable.Add(val);
                                    break;
                                }
                                if (operationForm[0].Contains("Insert"))
                                {
                                    if (currentLock.lockType.Equals("Read"))
                                    {
                                        Console.WriteLine("Tranzactia " + id + " a modificat lock-ul pe " + operationForm[0]);
                                        lockTable.UpdateLock(currentLock.id, "Write");
                                        detector.UpdateDeadlock(deadlock.id, "Write");
                                    }
                                    string val = operationForm[0] + ";" + null + ";" + null + ";" + operationForm[2];
                                    logTable.Add(val);
                                    break;
                                }
                                if (operationForm[0].Contains("Update"))
                                {
                                    if (currentLock.lockType.Equals("Read"))
                                    {
                                        Console.WriteLine("Tranzactia " + id + " a modificat lock-ul pe " + operationForm[0]);
                                        lockTable.UpdateLock(currentLock.id, "Write");
                                        detector.UpdateDeadlock(deadlock.id, "Write");
                                    }
                                    string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + operationForm[2];
                                    logTable.Add(val);
                                    break;
                                }
                                if (operationForm[0].Contains("Delete"))
                                {
                                    if (currentLock.lockType.Equals("Read"))
                                    {
                                        Console.WriteLine("Tranzactia " + id + " a modificat lock-ul pe " + operationForm[0]);
                                        lockTable.UpdateLock(currentLock.id, "Write");
                                        detector.UpdateDeadlock(deadlock.id, "Write");
                                    }
                                    string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + null;
                                    logTable.Add(val);
                                    break;
                                }

                            }
                            else
                            {
                                if (currentLock.GetType().Equals("Read"))
                                {
                                    if (operationForm[0].Contains("Get"))
                                    {
                                        Console.WriteLine("Tranzactia " + id + " a impartit lock-ul pe" + operationForm[0]);
                                        string val = operationForm[0] + ";" + resurceId + ";" + null + ";" + null;
                                        logTable.Add(val);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (deadlock != null)
                                    {
                                        if (!IsInWait(deadlock))
                                        {
                                            List<int> transWaitsLock = deadlock.transWaitsLock;
                                            transWaitsLock.Add(id);
                                            deadlock.transWaitsLock = transWaitsLock;
                                            detector.AddTransactionInWait(deadlock);
                                        }
                                        if (detector.DeadlockDetectionMecanism(id))
                                        {
                                            Console.WriteLine("Tranzactia " + id + " a esuat");
                                            transaction = providerTransaction.GetTransaction(id);
                                            transaction.status = "aboard";
                                            providerTransaction.UpdateTransaction(transaction);
                                            lockTable.EliberateLocks(id);
                                            detector.DeleteDeadlockData(id);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (i+1 == operations.Count)
                {
                    //luam fiecare operatie in parte, o executam, eliberam lock-ul sau executam operatile si eliberam lockul
                    //terminam tranzactia
                    foreach (string op in logTable)
                    {
                        string[] opFormat = op.Split(";");
                        if (opFormat[0].Contains("Get"))
                        {
                            if (opFormat[0].Contains("Student"))
                            {
                                Console.WriteLine("Tranzactia " + id + " a executat " + opFormat[0]);
                                //returnare Student
                                int idStudent = Convert.ToInt32(opFormat[1]);
                                operationObj.ExecuteMethodGet(opFormat[0],idStudent);
                                Student student = operationObj.student;
                                if (student != null)
                                {
                                    response = response + " Transaction " +id+" response " + " Student Id: " + student.id + " Nume: " + student.nume + " Nr Matricol: " + student.nrMatricol + " Varsta: " + student.varsta+" ;";
                                }
                            }
                            if (opFormat[0].Contains("Profesor"))
                            {
                                Console.WriteLine("Tranzactia " + id + " a executat " + opFormat[0]);
                                int idProfesor = Convert.ToInt32(opFormat[1]);
                                operationObj.ExecuteMethodGet(opFormat[0], idProfesor);
                                Profesor profesor = operationObj.profesor;
                                if (profesor != null)
                                {
                                    response = response+" Transaction " +id+" response " + " Profesor Id: " + profesor.id + " Nume: " + profesor.nume + " Specializare: " + profesor.specializare + " Varsta: " + profesor.varsta+" ;";
                                }
                            }
                            if (opFormat[0].Contains("Curs"))
                            {
                                Console.WriteLine("Tranzactia " + id + " a executat " + opFormat[0]);
                                int idCurs = Convert.ToInt32(opFormat[1]);
                                operationObj.ExecuteMethodGet(opFormat[0], idCurs);
                                Curs curs = operationObj.curs;
                                if (curs != null)
                                {
                                    response = response + " Transaction " + id + " response " + " Curs Id: " + curs.id + " Denumire: " + curs.denumire + " Descriere: " + curs.descriere + " NrStudenti: " + curs.nrStudenti+" ;";
                                }
                                }
                        }
                        if (opFormat[0].Contains("Insert"))
                        {
                            if (opFormat[0].Contains("Student"))
                            {
                                Console.WriteLine("Tranzactia " + this.id + " a executat " + opFormat[0]);
                                string[] studentForm = opFormat[3].Split("/");

                                //int id = Convert.ToInt32(studentForm[0]);
                                int idStudent = operationObj.GetNextIdStudent();
                                string nume = studentForm[1];
                                int varsta = Convert.ToInt32(studentForm[2]);
                                int nrMatricol = Convert.ToInt32(studentForm[3]);
                                Student student = new Student(idStudent, nume, varsta, nrMatricol);
                                operationObj.ExecuteMethodInsert(opFormat[0],student,null,null);
                            }
                            if (opFormat[0].Contains("Profesor"))
                            {
                                Console.WriteLine("Tranzactia " + this.id + " a executat " + opFormat[0]);
                                string[] profesorForm = opFormat[3].Split("/");
                                //int id = Convert.ToInt32(profesorForm[0]);
                                int idProfesor = operationObj.GetNextIdProfesor();
                                string nume = profesorForm[1];
                                int varsta = Convert.ToInt32(profesorForm[2]);
                                string specializare = profesorForm[3];
                                Profesor profesor = new Profesor(idProfesor, nume, varsta, specializare);
                                operationObj.ExecuteMethodInsert(opFormat[0], null, profesor, null);
                            }
                            if (opFormat[0].Contains("Curs"))
                            {
                                Console.WriteLine("Tranzactia " + this.id + " a executat " + opFormat[0]);
                                string[] cursForm = opFormat[3].Split("/");
                                //int id = Convert.ToInt32(cursForm[0]);
                                int idCurs = operationObj.GetNextIdCurs();
                                string denumire = cursForm[1];
                                int nrStudenti = Convert.ToInt32(cursForm[2]);
                                int idProfesor = Convert.ToInt32(cursForm[3]);
                                string descriere = cursForm[4];
                                Curs curs = new Curs(idCurs, denumire, nrStudenti, idProfesor, descriere);
                                operationObj.ExecuteMethodInsert(opFormat[0], null, null, curs);
                            }
                        }
                        if (opFormat[0].Contains("Update"))
                        {
                            if (opFormat[0].Contains("Student"))
                            {
                                Console.WriteLine("Tranzactia " + this.id + " a executat " + opFormat[0]);
                                int idStudent = Convert.ToInt32(opFormat[1]);
                                string[] studentForm = opFormat[3].Split("/");
                                int idStudent1 = Convert.ToInt32(studentForm[0]);
                                string nume = studentForm[1];
                                int varsta = Convert.ToInt32(studentForm[2]);
                                int nrMatricol = Convert.ToInt32(studentForm[3]);
                                Student student = new Student(idStudent1, nume, varsta, nrMatricol);
                                operationObj.ExecuteMethodUpdate(opFormat[0],idStudent, student, null, null);
                            }
                            if (opFormat[0].Contains("Profesor"))
                            {
                                Console.WriteLine("Tranzactia " + this.id + " a executat " + opFormat[0]);
                                int idProfesor = Convert.ToInt32(opFormat[1]);
                                string[] profesorForm = opFormat[3].Split("/");
                                int idProfesor1 = Convert.ToInt32(profesorForm[0]);
                                string nume = profesorForm[1];
                                int varsta = Convert.ToInt32(profesorForm[2]);
                                string specializare = profesorForm[3];
                                Profesor profesor = new Profesor(idProfesor1, nume, varsta, specializare);
                                operationObj.ExecuteMethodUpdate(opFormat[0],idProfesor, null, profesor, null);
                            }
                            if (opFormat[0].Contains("Curs"))
                            {
                                Console.WriteLine("Tranzactia " + this.id + " a executat " + opFormat[0]);
                                int idCurs = Convert.ToInt32(opFormat[1]);
                                string[] cursForm = opFormat[3].Split("/");
                                int idCurs1 = Convert.ToInt32(cursForm[0]);
                                string denumire = cursForm[1];
                                int nrStudenti = Convert.ToInt32(cursForm[2]);
                                int idProfesor = Convert.ToInt32(cursForm[3]);
                                string descriere = cursForm[4];
                                Curs curs = new Curs(idCurs1, denumire, nrStudenti, idProfesor, descriere);
                                operationObj.ExecuteMethodUpdate(opFormat[0],idCurs, null, null, curs);
                            }
                        }
                        if (opFormat[0].Contains("Delete"))
                        {
                            if (opFormat[0].Contains("Student"))
                            {
                                Console.WriteLine("Tranzactia " + id + " a executat " + opFormat[0]);
                                int idStudent = Convert.ToInt32(opFormat[1]);
                                operationObj.ExecuteMethodDelete(opFormat[0], idStudent);
                            }
                            if (opFormat[0].Contains("Profesor"))
                            {
                                Console.WriteLine("Tranzactia " + id + " a executat " + opFormat[0]);
                                int idProfesor = Convert.ToInt32(opFormat[1]);
                                operationObj.ExecuteMethodDelete(opFormat[0], idProfesor);
                            }
                            if (opFormat[0].Contains("Curs"))
                            {
                                Console.WriteLine("Tranzactia " + id + " a executat " + opFormat[0]);
                                int idCurs = Convert.ToInt32(opFormat[1]);
                                operationObj.ExecuteMethodDelete(opFormat[0], idCurs);
                            }
                        }
                    }
                }
            }
            Commit(id);
        }
        public void Commit(int idTrans)
        {
            Transaction transaction = providerTransaction.GetTransaction(id);
            transaction.status = "commit";
            providerTransaction.UpdateTransaction(transaction);
            lockTable.EliberateLocks(id);
            detector.DeleteDeadlockData(id);
        }
        public bool IsInWait(Deadlock currentDeadlock)
        {
            if (currentDeadlock != null)
            {
                List<int> transWait = currentDeadlock.transWaitsLock;
                foreach (int transId in transWait)
                {
                    if (transId == id)
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        public string GetTableName(string operationName)
        {
            if (operationName.Contains("SC"))
            {
                return "student_curs";
            }
            if (operationName.Contains("Profesor"))
            {
                return "profesor";
            }
            if (operationName.Contains("Student"))
            {
                return "student";
            }
            if (operationName.Contains("Curs"))
            {
                return "curs";
            }
            return null;
        }
        public int GetResourceId(string firstParamenter)
        {
            int id = Convert.ToInt32(firstParamenter);
            return id;
        }
        public string GetLockType(string operationName)
        {
            if (operationName.Contains("Get"))
            {
                return "Read";
            }
            else
            {
                return "Write";
            }
        }
        public void SetOperations(string ops)
        {
            operationFormatValue = ops;
            List<string> opts = new List<string>();
            string[] op = ops.Split("||");
            foreach (string o in op)
            {
                opts.Add(o);
                //Console.WriteLine(o);
            }
            operations = opts;
        }

    }
}
