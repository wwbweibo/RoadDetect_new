# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: LogLevel.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf.internal import enum_type_wrapper
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='LogLevel.proto',
  package='PythonCoreLib.Models',
  syntax='proto3',
  serialized_options=None,
  serialized_pb=_b('\n\x0eLogLevel.proto\x12\x14PythonCoreLib.Models*,\n\x08LogLevel\x12\x08\n\x04Info\x10\x00\x12\x0b\n\x07Warning\x10\x01\x12\t\n\x05\x45rror\x10\x02\x62\x06proto3')
)

_LOGLEVEL = _descriptor.EnumDescriptor(
  name='LogLevel',
  full_name='PythonCoreLib.Models.LogLevel',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='Info', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='Warning', index=1, number=1,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='Error', index=2, number=2,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=40,
  serialized_end=84,
)
_sym_db.RegisterEnumDescriptor(_LOGLEVEL)

LogLevel = enum_type_wrapper.EnumTypeWrapper(_LOGLEVEL)
Info = 0
Warning = 1
Error = 2


DESCRIPTOR.enum_types_by_name['LogLevel'] = _LOGLEVEL
_sym_db.RegisterFileDescriptor(DESCRIPTOR)


# @@protoc_insertion_point(module_scope)
