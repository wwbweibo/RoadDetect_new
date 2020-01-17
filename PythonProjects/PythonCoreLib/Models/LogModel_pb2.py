# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: LogModel.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


import PythonCoreLib.Models.LogLevel_pb2 as LogLevel__pb2
import PythonCoreLib.Models.ServiceType_pb2 as ServiceType__pb2


DESCRIPTOR = _descriptor.FileDescriptor(
  name='LogModel.proto',
  package='PythonCoreLib.Models',
  syntax='proto3',
  serialized_options=None,
  serialized_pb=_b('\n\x0eLogModel.proto\x12\x14PythonCoreLib.Models\x1a\x0eLogLevel.proto\x1a\x11ServiceType.proto\"\xcb\x01\n\x08LogModel\x12\x0f\n\x07LogTime\x18\x01 \x01(\t\x12\x12\n\nLogMessage\x18\x02 \x01(\t\x12\x30\n\x08LogLevel\x18\x03 \x01(\x0e\x32\x1e.PythonCoreLib.Models.LogLevel\x12\x17\n\x0fOriginServiceId\x18\x04 \x01(\t\x12<\n\x11OriginServiceType\x18\x05 \x01(\x0e\x32!.PythonCoreLib.Models.ServiceType\x12\x11\n\tException\x18\x06 \x01(\tb\x06proto3')
  ,
  dependencies=[LogLevel__pb2.DESCRIPTOR,ServiceType__pb2.DESCRIPTOR,])




_LOGMODEL = _descriptor.Descriptor(
  name='LogModel',
  full_name='PythonCoreLib.Models.LogModel',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='LogTime', full_name='PythonCoreLib.Models.LogModel.LogTime', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='LogMessage', full_name='PythonCoreLib.Models.LogModel.LogMessage', index=1,
      number=2, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='LogLevel', full_name='PythonCoreLib.Models.LogModel.LogLevel', index=2,
      number=3, type=14, cpp_type=8, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='OriginServiceId', full_name='PythonCoreLib.Models.LogModel.OriginServiceId', index=3,
      number=4, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='OriginServiceType', full_name='PythonCoreLib.Models.LogModel.OriginServiceType', index=4,
      number=5, type=14, cpp_type=8, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='Exception', full_name='PythonCoreLib.Models.LogModel.Exception', index=5,
      number=6, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=76,
  serialized_end=279,
)

_LOGMODEL.fields_by_name['LogLevel'].enum_type = LogLevel__pb2._LOGLEVEL
_LOGMODEL.fields_by_name['OriginServiceType'].enum_type = ServiceType__pb2._SERVICETYPE
DESCRIPTOR.message_types_by_name['LogModel'] = _LOGMODEL
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

LogModel = _reflection.GeneratedProtocolMessageType('LogModel', (_message.Message,), {
  'DESCRIPTOR' : _LOGMODEL,
  '__module__' : 'LogModel_pb2'
  # @@protoc_insertion_point(class_scope:PythonCoreLib.Models.LogModel)
  })
_sym_db.RegisterMessage(LogModel)


# @@protoc_insertion_point(module_scope)
