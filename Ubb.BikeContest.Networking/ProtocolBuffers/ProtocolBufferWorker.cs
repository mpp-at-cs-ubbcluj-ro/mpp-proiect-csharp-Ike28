﻿using Com.Ubb.Protocolbuffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ubb.BikeContest.Model;
using Ubb.BikeContest.Model.data;
using Ubb.BikeContest.Services;

using User = Ubb.BikeContest.Model.User;
using Participant = Ubb.BikeContest.Model.Participant;
using Google.Protobuf;

namespace Ubb.BikeContest.Networking.ProtocolBuffers
{
    public class ProtocolBufferWorker : IMainObserver
    {
        private readonly IContestServices _server;
        private readonly TcpClient _connection;
        private readonly NetworkStream _stream;
        private volatile bool _connected;

        public ProtocolBufferWorker(IContestServices server, TcpClient connection)
        {
            _server = server;
            _connection = connection;

            try
            {
                _stream = _connection.GetStream();
                _connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void ParticipantAdded(Participant participant)
        {
            throw new NotImplementedException();
        }

        public void RaceEntriesAdded(List<RaceDto> races)
        {
            throw new NotImplementedException();
        }

        private BikeContestResponse HandleRequest(BikeContestRequest request)
        {
            BikeContestRequest.Types.Type type = request.Type;

            switch (type)
            {
                case BikeContestRequest.Types.Type.Login:
                    Console.WriteLine("> handling login request...");
                    string username = ProtocolBuilderUtils.GetUsername(request);
                    string passwordToken = ProtocolBuilderUtils.GetPasswordToken(request);

                    try
                    {
                        lock (_server)
                        {
                            User connectedUser = _server.Login(username, passwordToken, this);
                            return ProtocolBuilderUtils.CreateOkResponse(connectedUser);
                        }
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }

                case BikeContestRequest.Types.Type.Logout:
                    Console.WriteLine("> handling logout request...");
                    User user = ProtocolBuilderUtils.GetUser(request);

                    try
                    {
                        lock (_server)
                        {
                            _server.Logout(user, this);
                        }

                        _connected = false;
                        return ProtocolBuilderUtils.CreateOkResponse();
                    }
                    catch (Exception e)
                    {
                        return ProtocolBuilderUtils.CreateErrorResponse(e.Message);
                    }
            }
            return null;
        }

        private void SendResponse(BikeContestResponse response)
        {
            Console.WriteLine("Sending response: " + response);

            lock (_stream)
            {
                response.WriteDelimitedTo(_stream);
                _stream.Flush();
            }
        }

        public virtual void Run()
        {
            while (_connected)
            {
                try
                {
                    BikeContestRequest request = BikeContestRequest.Parser.ParseDelimitedFrom(_stream);
                    BikeContestResponse response = HandleRequest(request);

                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            try
            {
                _stream.Close();
                _connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
